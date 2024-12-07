using HTApp.Core.API;
using HTApp.Core.API.Models.RepoModels;
using System.Security.Cryptography.X509Certificates;
using static HTApp.Core.API.ApplicationInvariants;

namespace HTApp.Core.Services;

public class SessionService : ISessionService
{
    ISessionRepository repo;
    IUnitOfWork unitOfWork;

    IUserDataService userDataService;

    IGoodHabitService ghService;
    IBadHabitService bhService;
    ITreatService trService;

    private HashSet<ISessionObserver> makeTransactionSubscribers;

    public SessionService(ISessionRepository repo, IUnitOfWork unitOfWork,
        IUserDataService userDataService,
        IGoodHabitService ghService, IBadHabitService bhService, ITreatService trService)
    {
        this.repo = repo;
        this.unitOfWork = unitOfWork;

        this.userDataService = userDataService;

        makeTransactionSubscribers = new();

        this.ghService = ghService;
        this.ghService.SubscribeToStatusChange(this);

        this.bhService = bhService;
        this.bhService.SubscribeToStatusChange(this);

        this.trService = trService;
        this.trService.SubscribeToStatusChange(this);
    }

    ~SessionService()
    {
        ghService.UnsubscribeToStatusChange(this);
        bhService.UnsubscribeToStatusChange(this);
        trService.UnsubscribeToStatusChange(this);
    }

    public async ValueTask<Response<SessionModel>> GetLastSession(string userId, bool isNotFinished)
    {
        SessionModel? model = await repo.GetLastSession(userId, isNotFinished);

        if(model is null)
        {
            return new Response<SessionModel>(ResponseCode.NotFound, "Not Found.");
        }

        return new Response<SessionModel>(ResponseCode.Success, "Success.", model);
    }

    public async ValueTask<ResponseStruct<int>> GetLastSessionId(string userId, bool mustNotBeFinished)
    {
        int? id = null;
        if(mustNotBeFinished)
        {
            var model = await repo.GetIsLastSessionFinished(userId);
            if(model is not null && !model.IsFinished)
            {
                id = model.Id;
            }
        }
        else
        {
            id = await repo.GetLastSessionId(userId);
        }

        if(id is null)
        {
            return new ResponseStruct<int>(ResponseCode.NotFound, "Not Found.");
        }

        return new ResponseStruct<int>(ResponseCode.Success, "Success", id.Value);
    }

    public async ValueTask<Response> UpdateGoodHabit(int id, bool success, string userId)
    {
        //Don't allow to change if not in progress
        SessionIsFinishedModel? lastSession = await repo.GetIsLastSessionFinished(userId);
        if(lastSession is null || lastSession.IsFinished)
        {
            return new Response(ResponseCode.InvalidOperation, "Invalid Operation.");
        }

        //Is it in the session
        if(!(await repo.GetGoodHabitIds(userId))!.Contains(id))
        {
            return new Response(ResponseCode.NotFound, "Not Found");
        }

        //Get useful data and also it checks for existance and authorization, which is probably not necessary.
        Response<GoodHabitLogicModel> lModelResponse = await ghService.GetLogicModel(id, userId);
        if(lModelResponse.Code != ResponseCode.Success)
        {
            return new Response(lModelResponse.Code, lModelResponse.Message);
        }
        GoodHabitLogicModel logicModel = lModelResponse.Payload!;

        //check for redundancy
        bool oldSuccess = (await repo.GetGoodHabitCompleted(id, userId)).Value;
        if(oldSuccess == success)
        {
            return new Response(ResponseCode.Success, "Nothing changed.");
        }


        MakeTransactionInfo info = new MakeTransactionInfo()
        {
            SessionId = lastSession.Id,
            UserId = userId,
        };
        if(success)
        {
            info.Amount = logicModel.CreditsSuccess;
            info.TransactionType = TransactionTypesEnum.GoodHabitSuccess;
        }
        else
        {
            info.Amount = -logicModel.CreditsSuccess;
            info.TransactionType = TransactionTypesEnum.GoodHabitSuccessCancel;
        }

        bool hurray = await repo.UpdateGoodHabit(id, success, userId);

        //notify -----
        var bigResponse = Response.AggregateErrors(await NotifyMakeTransaction(info));
        if(bigResponse.Code == ResponseCode.ServiceError)
        {
            return bigResponse;
        }

        hurray = hurray && await unitOfWork.SaveChangesAsync();

        if(!hurray)
        {
            return new Response(ResponseCode.RepositoryError, "Something went wrong. Please try again.");
        }

        return new Response(ResponseCode.Success, "Success.");
    }

    public async ValueTask<Response> UpdateBadHabit(int id, bool fail, string userId)
    {
        //Don't allow to change if not in progress
        SessionIsFinishedModel? lastSession = await repo.GetIsLastSessionFinished(userId);
        if(lastSession is null || lastSession.IsFinished)
        {
            return new Response(ResponseCode.InvalidOperation, "Invalid Operation.");
        }

        //Is it in the session
        if(!(await repo.GetBadHabitIds(userId))!.Contains(id))
        {
            return new Response(ResponseCode.NotFound, "Not Found");
        }

        //Get useful data and also it checks for existance and authorization, which is probably not necessary.
        Response<BadHabitLogicModel> lModelResponse = await bhService.GetLogicModel(id, userId);
        if(lModelResponse.Code != ResponseCode.Success)
        {
            return new Response(lModelResponse.Code, lModelResponse.Message);
        }
        BadHabitLogicModel logicModel = lModelResponse.Payload!;

        //check for redundancy
        bool oldFail = (await repo.GetBadHabitFailed(id, userId)).Value;
        if(oldFail == fail)
        {
            return new Response(ResponseCode.Success, "Nothing changed.");
        }


        MakeTransactionInfo info = new MakeTransactionInfo()
        {
            SessionId = lastSession.Id,
            UserId = userId,
        };
        if(fail)
        {
            info.Amount = -logicModel.CreditsFail;
            info.TransactionType = TransactionTypesEnum.BadHabitFail;
        }
        else
        {
            info.Amount = logicModel.CreditsFail;
            info.TransactionType = TransactionTypesEnum.BadHabitFailCancel;
        }

        bool hurray = await repo.UpdateBadHabit(id, fail, userId);

        //notify -----
        var bigResponse = Response.AggregateErrors(await NotifyMakeTransaction(info));
        if(bigResponse.Code == ResponseCode.ServiceError)
        {
            return bigResponse;
        }

        hurray = hurray && await unitOfWork.SaveChangesAsync();

        if(!hurray)
        {
            return new Response(ResponseCode.RepositoryError, "Something went wrong. Please try again.");
        }

        return new Response(ResponseCode.Success, "Success.");
    }

    public async ValueTask<Response> BuyTreat(int id, string userId)
    {
        //Don't allow to change if not in progress
        SessionIsFinishedModel? lastSession = await repo.GetIsLastSessionFinished(userId);
        if(lastSession is null || lastSession.IsFinished)
        {
            return new Response(ResponseCode.InvalidOperation, "Invalid Operation.");
        }

        //Is it in the session
        if(!(await repo.GetTreatIds(userId))!.Contains(id))
        {
            return new Response(ResponseCode.NotFound, "Not Found");
        }

        //Check if it makes sense
        byte unitsLeft = (await repo.GetTreatUnitsLeft(id, userId)).Value;
        if(unitsLeft == 0)
        {
            return new Response(ResponseCode.InvalidOperation, "Cannot buy more. Quantity is 0");
        }

        //Get useful data and also it checks for existance and authorization, which is probably not necessary.
        Response<TreatLogicModel> lModelResponse = await trService.GetLogicModel(id, userId);
        if(lModelResponse.Code != ResponseCode.Success)
        {
            return new Response(lModelResponse.Code, lModelResponse.Message);
        }
        TreatLogicModel logicModel = lModelResponse.Payload!;

        MakeTransactionInfo info = new MakeTransactionInfo()
        {
            Amount = -logicModel.Price,
            TransactionType = TransactionTypesEnum.BuyingTreat,
            SessionId = lastSession.Id,
            UserId = userId,
        };

        bool hurray = await repo.DecrementTreat(id, userId);

        //notify -----
        var bigResponse = Response.AggregateErrors(await NotifyMakeTransaction(info));
        if(bigResponse.Code == ResponseCode.ServiceError)
        {
            return bigResponse;
        }

        hurray = hurray && await unitOfWork.SaveChangesAsync();

        if(!hurray)
        {
            return new Response(ResponseCode.RepositoryError, "Something went wrong. Please try again.");
        }

        return new Response(ResponseCode.Success, "Success.");
    }

    public async ValueTask<Response> RefundTreat(int id, string userId)
    {
        //Don't allow to change if not in progress
        SessionIsFinishedModel? lastSession = await repo.GetIsLastSessionFinished(userId);
        if(lastSession is null || lastSession.IsFinished)
        {
            return new Response(ResponseCode.InvalidOperation, "Invalid Operation.");
        }

        //Is it in the session
        if(!(await repo.GetTreatIds(userId))!.Contains(id))
        {
            return new Response(ResponseCode.NotFound, "Not Found");
        }

        //Get useful data and also it checks for existance and authorization, which is probably not necessary.
        Response<TreatLogicModel> lModelResponse = await trService.GetLogicModel(id, userId);
        if(lModelResponse.Code != ResponseCode.Success)
        {
            return new Response(lModelResponse.Code, lModelResponse.Message);
        }
        TreatLogicModel logicModel = lModelResponse.Payload!;

        //Check if it makes sense
        byte unitsLeft = (await repo.GetTreatUnitsLeft(id, userId)).Value;
        if(unitsLeft == logicModel.UnitsPerSession)
        {
            return new Response(ResponseCode.InvalidOperation, "Cannot refund more. Quantity is Max");
        }
        byte refunds = (await repo.GetJustRefunds(userId)).Value;
        if(refunds == 0)
        {
            return new Response(ResponseCode.InvalidOperation, "Cannot refund more. No refunds left");
        }


        MakeTransactionInfo info = new MakeTransactionInfo()
        {
            Amount = logicModel.Price,
            TransactionType = TransactionTypesEnum.RefundTreat,
            SessionId = lastSession.Id,
            UserId = userId,
        };

        bool hurray = await repo.IncrementTreat(id, userId) && await repo.DecrementRefunds(userId);

        //notify -----
        var bigResponse = Response.AggregateErrors(await NotifyMakeTransaction(info));
        if(bigResponse.Code == ResponseCode.ServiceError)
        {
            return bigResponse;
        }

        hurray = hurray && await unitOfWork.SaveChangesAsync();

        if(!hurray)
        {
            return new Response(ResponseCode.RepositoryError, "Something went wrong. Please try again.");
        }

        return new Response(ResponseCode.Success, "Success.");
    }

    public async ValueTask<Response> StartNewSession(string userId)
    {
        //Don't allow if already in session
        SessionIsFinishedModel? lastSession = await repo.GetIsLastSessionFinished(userId);
        if(lastSession is not null && !lastSession.IsFinished)
        {
            return new Response(ResponseCode.InvalidOperation, "Invalid Operation. Last session is not finished");
        }

        SessionAddModel model = new SessionAddModel
        {
            StartDate = DateTime.Now,
            UserId = userId,
        };

        byte refunds = (await userDataService.GetRefundsPerSession(userId)).Payload;
        model.Refunds = refunds;

        int[] gh = (await ghService.GetAllIds(userId, true)).Payload!;
        model.GoodHabitIds = gh;

        int[] bh = (await bhService.GetAllIds(userId)).Payload!;
        model.BadHabitIds = bh;

        Tuple<int, byte>[] tr = (await trService.GetAllIdAndUnitsLeftPairs(userId)).Payload!;
        model.TreatIdUnitPerSessionPairs = tr;

        bool success = await repo.StartNewSession(model) && await unitOfWork.SaveChangesAsync();

        if(!success)
        {
            return new Response(ResponseCode.RepositoryError, "Something went wrong. Please try again.");
        }

        return new Response(ResponseCode.Success, "Success.");
    }

    public async ValueTask<Response> FinishCurrentSession(string userId)
    {
        //Don't allow to change if not in progress
        SessionIsFinishedModel? lastSession = await repo.GetIsLastSessionFinished(userId);
        if(lastSession is null || lastSession.IsFinished)
        {
            return new Response(ResponseCode.InvalidOperation, "Invalid Operation. Last session is not finished");
        }

        bool success = await repo.FinishCurrentSession(userId);

        SessionHabitCreditsModel[] ghFailed = (await repo.GetGoodHabitsFailed(userId))!;
        List<Response> ghResponses = new();
        foreach(var gh in ghFailed)
        {
            MakeTransactionInfo info = new MakeTransactionInfo()
            {
                SessionId = lastSession.Id,
                UserId = userId,
                Amount = -gh.Credits,
                TransactionType = TransactionTypesEnum.GoodHabitFail
            };
            //notify -----
            ghResponses.AddRange(await NotifyMakeTransaction(info));
        }

        SessionHabitCreditsModel[] bhSuccess = (await repo.GetBadHabitsSuccess(userId))!;
        List<Response> bhResponses = new();
        foreach(var bh in bhSuccess)
        {
            MakeTransactionInfo info = new MakeTransactionInfo()
            {
                SessionId = lastSession.Id,
                UserId = userId,
                Amount = bh.Credits,
                TransactionType = TransactionTypesEnum.BadHabitSuccess
            };
            //notify -----
            bhResponses.AddRange(await NotifyMakeTransaction(info));
        }

        var bigResponse = Response.AggregateErrors(ghResponses.Concat(bhResponses));
        if(bigResponse.Code == ResponseCode.ServiceError)
        {
            return bigResponse;
        }

        success = success && await unitOfWork.SaveChangesAsync();

        if(!success)
        {
            return new Response(ResponseCode.RepositoryError, "Something went wrong. Please try again.");
        }

        return new Response(ResponseCode.Success, "Success.");

    }

    async ValueTask<Response> IGoodHabitObserver.NotifyWhenStatusChange(bool isActive, string userId)
    {
        //Don't allow to change if not in progress
        SessionIsFinishedModel? lastSession = await repo.GetIsLastSessionFinished(userId);
        if(lastSession is null || lastSession.IsFinished)
        {
            return new Response(ResponseCode.InvalidOperation, "Invalid Operation.");
        }

        if(!isActive)
        {
            return new Response(ResponseCode.Success, "Nothing changed.");
        }
        int[] ids = (await ghService.GetAllIds(userId, true)).Payload!;

        bool success = await repo.UpdateGoodHabits(ids, userId);

        if(!success)
        {
            return new Response(ResponseCode.RepositoryError, "Something went wrong. Please try again.");
        }

        return new Response(ResponseCode.Success, "Success.");
    }

    async ValueTask<Response> IBadHabitObserver.NotifyWhenStatusChange(string userId)
    {
        //Don't allow to change if not in progress
        SessionIsFinishedModel? lastSession = await repo.GetIsLastSessionFinished(userId);
        if(lastSession is null || lastSession.IsFinished)
        {
            return new Response(ResponseCode.InvalidOperation, "Invalid Operation.");
        }

        int[] ids = (await bhService.GetAllIds(userId)).Payload!;

        bool success = await repo.UpdateBadHabits(ids, userId);

        if(!success)
        {
            return new Response(ResponseCode.RepositoryError, "Something went wrong. Please try again.");
        }

        return new Response(ResponseCode.Success, "Success.");
    }

    async ValueTask<Response> ITreatObserver.NotifyWhenStatusChange(string userId)
    {
        //Don't allow to change if not in progress
        SessionIsFinishedModel? lastSession = await repo.GetIsLastSessionFinished(userId);
        if(lastSession is null || lastSession.IsFinished)
        {
            return new Response(ResponseCode.InvalidOperation, "Invalid Operation.");
        }

        var pairs = (await trService.GetAllIdAndUnitsLeftPairs(userId)).Payload!;

        bool success = await repo.UpdateTreats(pairs, userId);

        if(!success)
        {
            return new Response(ResponseCode.RepositoryError, "Something went wrong. Please try again.");
        }

        return new Response(ResponseCode.Success, "Success.");
    }

    public async Task<Response[]> NotifyMakeTransaction(MakeTransactionInfo info)
    {
        List<Response> list = new List<Response>();
        foreach(var sub in makeTransactionSubscribers)
        {
            list.Add(await sub.NotifyWhenMakeTransaction(info));
        }
        return list.ToArray();
    }

    public void SubscribeToMakeTransaction(ISessionObserver observer)
    {
        if(observer == null)
        {
            throw new ArgumentNullException();
        }
        //should handle duplicates as a HashSet.
        makeTransactionSubscribers.Add(observer);
    }

    public void UnsubscribeToMakeTransaction(ISessionObserver observer)
    {
        if(observer == null)
        {
            throw new ArgumentNullException();
        }
        makeTransactionSubscribers.Remove(observer);
    }
}
