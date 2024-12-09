using HTApp.Core.API;
using static HTApp.Core.API.ApplicationInvariants;

namespace HTApp.Core.Services;

public class TransactionService : ITransactionService
{
    private static HashSet<string> transactionTypes = Enum.GetNames<TransactionTypesEnum>().ToHashSet();

    ITransactionRepository repo;
    IUnitOfWork unitOfWork;

    IUserDataService userDataService;

    ISessionService sessionService;

    public TransactionService(ITransactionRepository repo, IUnitOfWork unitOfWork, IUserDataService userDataService, ISessionService sessionService)
    {
        this.repo = repo;
        this.unitOfWork = unitOfWork;

        this.userDataService = userDataService;

        this.sessionService = sessionService;
        this.sessionService.SubscribeToMakeTransaction(this);
    }

    ~TransactionService()
    {
        sessionService.UnsubscribeToMakeTransaction(this);
    }

    public async ValueTask<Response<TransactionServiceResponse>> GetAll(string userId, int pageCount, int pageNumber, string filterTypeName = "", bool fromLastSession = false)
    {
        if(!transactionTypes.Contains(filterTypeName))
        {
            filterTypeName = "";
        }

        int? lastSessionId = null;
        if(fromLastSession)
        {
            ResponseStruct<int> resp = await sessionService.GetLastSessionId(userId, false);
            if(resp.Code == ResponseCode.NotFound)
            {
                //probably do nothing??
            }
            else if(resp.Code != ResponseCode.Success)
            {
                throw new Exception("Unhandled code in getting CurrentSessionId in GetAll from TransactionService.");
            }
            lastSessionId = resp.Payload;
        }

        TransactionOptions opt = new TransactionOptions
        {
            AdditionalEntries = 1, //used to check whether we have a next page
            FilterTypeName = filterTypeName,
            FromSessionId = lastSessionId,
        };

        //check if pageCount is too big. And also prevent pageNumber == 0;
        double count = await repo.GetCount(userId, filterTypeName, lastSessionId);
        pageNumber = (int)Math.Max(1, Math.Min(pageNumber, Math.Ceiling(count/pageCount)));

        TransactionModel[] modelsPlusOne = await repo.GetAll(userId, pageCount, pageNumber, opt);

        TransactionServiceResponse response = new TransactionServiceResponse()
        {
            HasNext = modelsPlusOne.Length == pageCount + 1, //the extra one is like a sneak peak to the next page if it exist
            PageNumber = pageNumber,
        };
        response.Models = response.HasNext ? modelsPlusOne.SkipLast(1).ToArray() : modelsPlusOne;

        return new Response<TransactionServiceResponse>(ResponseCode.Success, "Success.", response);
    }

    public async ValueTask<Response<TransactionServiceResponse>> GetAllLatest(string userId, int pageCount, string filterTypeName = "", bool fromLastSession = false)
    {
        return await GetAll(userId, pageCount, int.MaxValue, filterTypeName, fromLastSession);
    }

    //public async ValueTask<ResponseStruct<int>> GetCount(string userId)
    //{
    //    return new ResponseStruct<int>(ResponseCode.Success, "Success.", await repo.GetCount(userId));
    //}

    public async ValueTask<Response<string[]>> GetTypeNames(string userId, string filterTypeName = "", bool fromLastSession = false)
    {
        if(!transactionTypes.Contains(filterTypeName))
        {
            filterTypeName = "";
        }

        int? lastSessionId = null;
        if(fromLastSession)
        {
            ResponseStruct<int> resp = await sessionService.GetLastSessionId(userId, false);
            if(resp.Code == ResponseCode.NotFound)
            {
                //probably do nothing??
            }
            else if(resp.Code != ResponseCode.Success)
            {
                throw new Exception("Unhandled code in getting CurrentSessionId in GetAll from TransactionService.");
            }
            lastSessionId = resp.Payload;
        }

        return new Response<string[]>(ResponseCode.Success, "Success.", await repo.GetUsedTypeNames(userId, filterTypeName, lastSessionId));
    }

    public async ValueTask<Response> Add(TransactionInputModel model, string userId, bool saveChanges = true)
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
        //Also Observer Pattern doesn't really work here, because I need the payload, so it's not 1-way communication.
        Response<AppendCreditsResponse> UserCreditsResponse = await userDataService.AppendCredits(model.Amount, userId, false);
        if(UserCreditsResponse.Code == ResponseCode.RepositoryError)
        {
            return new Response(ResponseCode.RepositoryError, "Something went wrong. Please try again.");
        }
        if(UserCreditsResponse.Code != ResponseCode.Success)
        {
            throw new Exception("Unhandled Response Case.");
        }

        //Update the model according to the data returned
        //The cap might be hit, that's why.
        model.Amount = UserCreditsResponse.Payload!.Diff;

        bool success = await repo.Add(model) && 
            (saveChanges ? await unitOfWork.SaveChangesAsync() : true);

        if(!success)
        {
            return new Response(ResponseCode.RepositoryError, "Something went wrong. Please try again.");
        }

        return new Response(ResponseCode.Success, "Success.");
    }

    public async ValueTask<Response> AddManual(int amount, string userId)
    {
        //Get current session Id if any
        int? lastSessionId = null;

        //lastSessionId is null if no active session
        var response = await sessionService.GetLastSessionId(userId, true);
        if(response.Code == ResponseCode.NotFound){}
        else if(response.Code == ResponseCode.Success)
        {
            lastSessionId = response.Payload;
        }
        else
        {
            throw new Exception("Unhandled response code");
        }

        TransactionInputModel model = new TransactionInputModel
        {
            Type = nameof(TransactionTypesEnum.Manual),
            Amount = amount,
            SessionId = lastSessionId
        };

        return await Add(model, userId);
    }

    public ValueTask<Response> NotifyWhenMakeTransaction(MakeTransactionInfo info)
    {
        TransactionInputModel model = new TransactionInputModel
        {
            Type = Enum.GetName(info.TransactionType) ?? "",
            Amount = info.Amount,
            SessionId = info.SessionId,
        };

        return Add(model, info.UserId, false);
    }
}
