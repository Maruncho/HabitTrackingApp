using HTApp.Core.API;
using static HTApp.Core.API.ApplicationInvariants;

namespace HTApp.Core.Services;

public class TransactionService : ITransactionService
{
    private static HashSet<string> transactionTypes = TransactionTypes.ToHashSet();

    ITransactionRepository repo { get; init; }
    IUnitOfWork unitOfWork;
    IUserDataService userDataService { get; set; }

    public TransactionService(ITransactionRepository repo, IUnitOfWork unitOfWork, IUserDataService userDataService)
    {
        this.repo = repo;
        this.unitOfWork = unitOfWork;
        this.userDataService = userDataService;
    }

    public async ValueTask<Response<TransactionServiceResponse>> GetAll(string userId, int pageCount, int pageNumber, string filterTypeName = "")
    {

        if(!transactionTypes.Contains(filterTypeName))
        {
            filterTypeName = "";
        }

        TransactionModel[] modelsPlusOne = await repo.GetAll(userId, pageCount, pageNumber, 1, filterTypeName);
        if(modelsPlusOne.Length == 0 && pageNumber != 1)
        {
            //some weird recursion ;p
            return await GetAllLatest(userId, pageCount);
        }

        TransactionServiceResponse response = new TransactionServiceResponse()
        {
            HasNext = modelsPlusOne.Length == pageCount + 1, //the extra one is like a sneak peak to the next page if it exist
            PageNumber = pageNumber,
        };
        response.Models = response.HasNext ? modelsPlusOne.SkipLast(1).ToArray() : modelsPlusOne;

        return new Response<TransactionServiceResponse>(ResponseCode.Success, "Success.", response);
    }

    public async ValueTask<Response<TransactionServiceResponse>> GetAllLatest(string userId, int pageCount, string filterTypeName = "")
    {
        double count = await repo.GetCount(userId);
        int pageNumber = (int)Math.Max(1, Math.Ceiling(count/pageCount));
        return await GetAll(userId, pageCount, pageNumber, filterTypeName);
    }

    public async ValueTask<ResponseStruct<int>> GetCount(string userId)
    {
        return new ResponseStruct<int>(ResponseCode.Success, "Success.", await repo.GetCount(userId));
    }

    public async ValueTask<Response<string[]>> GetTypeNames(string userId)
    {
        return new Response<string[]>(ResponseCode.Success, "Success.", await repo.GetUsedTypeNames(userId));
    }

    public async ValueTask<Response> Add(TransactionInputModel model, string userId)
    {
        if(model.Amount < TransactionAmountMin || model.Amount > TransactionAmountMax)
        {
            return new Response(ResponseCode.InvalidField, TransactionAmountError);
        }
        if(!transactionTypes.Contains(model.Type))
        {
            return new Response(ResponseCode.InvalidField, "Invalid Transaction Type. This is a dev mistake. He sucks if you see this.");
        }

        model.UserId = userId;

        //Update the credits
        //Note that it doesn't save changes.
        Response<AppendCreditsResponse> UserCreditsResponse = await userDataService.AppendCredits(model.Amount, userId, false);
        if(UserCreditsResponse.Code == ResponseCode.RepositoryError)
        {
            throw new Exception("Something went wrong. Please try again.");
        }
        if(UserCreditsResponse.Code != ResponseCode.Success)
        {
            throw new Exception("Unhandled Response Case.");
        }

        //Update the model according to the data returned
        model.Amount = UserCreditsResponse.Payload!.Diff;

        bool success = await repo.Add(model) && await unitOfWork.SaveChangesAsync();
        if(!success)
        {
            return new Response(ResponseCode.RepositoryError, "Something went wrong. Please try again.");
        }

        return new Response(ResponseCode.Success, "Success.");
    }

    public ValueTask<Response> AddManual(int amount, string userId)
    {
        TransactionInputModel model = new TransactionInputModel
        {
            Type = nameof(TransactionTypesEnum.Manual),
            Amount = amount,
        };

        return Add(model, userId);
    }
}
