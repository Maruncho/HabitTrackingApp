using HTApp.Core.API;
using System.Security.Cryptography;
using static HTApp.Core.API.ApplicationInvariants;

namespace HTApp.Core.Services;

public class UserDataService : IUserDataService
{
    IUserDataRepository repo;
    IUnitOfWork unitOfWork;

    private int? oldCreditsAcum = null;

    public UserDataService(IUserDataRepository repo, IUnitOfWork unitOfWork)
    {
        this.repo = repo;
        this.unitOfWork = unitOfWork;
    }

    public async ValueTask<Response<UserDataDump>> GetUserData(string userId)
    {
        return new Response<UserDataDump>(ResponseCode.Success, "Success.", await repo.GetEverything(userId));
    }

    public async ValueTask<ResponseStruct<int>> GetCredits(string userId)
    {
        return new ResponseStruct<int>(ResponseCode.Success, "Success.", await repo.GetCredits(userId));
    }

    public async ValueTask<Response<AppendCreditsResponse>> AppendCredits(int credits, string userId, bool saveChanges = true)
    {
        int oldCredits;
        //so we can track multiple appendages before SaveChanges()
        if(oldCreditsAcum is null)
        {
            oldCreditsAcum = await repo.GetCredits(userId);
        }
        oldCredits = oldCreditsAcum.Value;

        (int newCredits, bool capped) = CapIfOverflow(oldCredits, credits);

        await repo.SetCredits(userId, newCredits);

        if (saveChanges)
        {
            bool success = await unitOfWork.SaveChangesAsync();
            if (!success)
            {
                return new Response<AppendCreditsResponse>(ResponseCode.RepositoryError, "Something went wrong. Please try again.");
            }
            //update from db for the next call
            oldCreditsAcum = null;
        }
        else
        {
            oldCreditsAcum = newCredits;
        }
        int diff = newCredits - oldCredits;
        return new Response<AppendCreditsResponse>(ResponseCode.Success, "Success", new AppendCreditsResponse { NewAmount = newCredits, Diff = diff, Capped = capped});
    }

    public async ValueTask<ResponseStruct<byte>> GetRefundsPerSession(string userId)
    {
        return new ResponseStruct<byte>(ResponseCode.Success, "Success.", await repo.GetRefundsPerSession(userId));
    }

    public async ValueTask<Response> SetRefundsPerSession(byte newRefunds, string userId)
    {
        if(newRefunds < UserDataRefundsMin || newRefunds > UserDataRefundsMax)
        {
            return new Response(ResponseCode.InvalidField, UserDataRefundsError);
        }

        await repo.SetRefundsPerSession(userId, newRefunds);
        bool success = await unitOfWork.SaveChangesAsync();
        if(!success)
        {
            return new Response(ResponseCode.RepositoryError, "Something went wrong. Please try again.");
        }

        return new Response(ResponseCode.Success, "Success");
    }

    private (int newCredits, bool capped) CapIfOverflow(int credits, int addCredits)
    {
        try
        {
            checked
            {
                int result = credits + addCredits;

                if(result > UserDataCreditsMax)
                {
                    return (UserDataCreditsMax, true);
                }
                if(result < UserDataCreditsMin)
                {
                    return (UserDataCreditsMin, true);
                }

                return (result, false);
            }
        }
        catch(OverflowException)
        {
            if(credits >= 0)
            {
                return (int.MaxValue, true);
            }
            else
            {
                return (int.MinValue, true);
            }
        }
    }
}
