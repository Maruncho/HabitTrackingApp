using HTApp.Core.API;
using static HTApp.Core.API.ApplicationInvariants;

namespace HTApp.Core.Services;

public class GoodHabitService : IGoodHabitService
{
    IGoodHabitRepository repo;
    IUnitOfWork unitOfWork;

    private HashSet<IGoodHabitObserver> changeStatusSubscribers;

    public GoodHabitService(IGoodHabitRepository repo, IUnitOfWork unitOfWork)
    {
        this.repo = repo;
        this.unitOfWork = unitOfWork;

        changeStatusSubscribers = new();
    }

    public async Task<Response> Add(GoodHabitInputModel model, string userId)
    {
        if(model.Name.Length < GoodHabitNameLengthMin || model.Name.Length > GoodHabitNameLengthMax)
        {
            return new Response(ResponseCode.InvalidField, GoodHabitNameLengthError);
        }

        if(model.CreditsSuccess < GoodHabitCreditsSuccessMin || model.CreditsSuccess > GoodHabitCreditsSuccessMax)
        {
            return new Response(ResponseCode.InvalidField, GoodHabitNameCreditsSuccessError);
        }

        if(model.CreditsFail < GoodHabitCreditsFailMin || model.CreditsFail > GoodHabitCreditsFailMax)
        {
            return new Response(ResponseCode.InvalidField, GoodHabitNameCreditsFailError);
        }

        model.UserId = userId;

        bool success = await repo.Add(model) && await unitOfWork.SaveChangesAsync();
        if(!success)
        {
            return new Response(ResponseCode.RepositoryError, "Something went wrong. Please try again.");
        }

        //notify -----
        var bigResponse = Response.AggregateErrors(await NotifyStatusChange(true /*shouldn't matter*/, userId));
        if(bigResponse.Code == ResponseCode.ServiceError)
        {
            //do nothing. The current architecture makes it hard to inform the user about errors from elsewhere in a general way.
        }
        await unitOfWork.SaveChangesAsync();

        return new Response(ResponseCode.Success, "Success");
    }

    public async Task<Response> Delete(int id, string userId)
    {
        bool exists = await repo.Exists(id);
        if(!exists)
        {
            return new Response(ResponseCode.NotFound, "Not Found");
        }

        bool allowed = await repo.IsOwnerOf(id, userId);
        if(!allowed)
        {
            return new Response(ResponseCode.Unauthorized, "Unauthorized");
        }

        bool success = await repo.Delete(id) && await unitOfWork.SaveChangesAsync();
        if(!success)
        {
            return new Response(ResponseCode.RepositoryError, "Something went wrong. Please try again.");
        }

        //notify -----
        var bigResponse = Response.AggregateErrors(await NotifyStatusChange(true /*shouldn't matter*/, userId));
        if(bigResponse.Code == ResponseCode.ServiceError)
        {
            //do nothing. The current architecture makes it hard to inform the user about errors from elsewhere in a general way.
        }
        await unitOfWork.SaveChangesAsync();

        return new Response(ResponseCode.Success, "Success");
    }

    public async Task<Response<GoodHabitModel[]>> GetAll(string userId)
    {
        return new Response<GoodHabitModel[]>(ResponseCode.Success, "Success.", await repo.GetAll(userId));
    }

    public async Task<Response<int[]>> GetAllIds(string userId, bool onlyActive = false)
    {
        return new Response<int[]>(ResponseCode.Success, "Success.", await repo.GetAllIds(userId, onlyActive));
    }

    public async Task<Response<GoodHabitInputModel>> GetInputModel(int id, string userId)
    {
        bool exists = await repo.Exists(id);
        if(!exists)
        {
            return new Response<GoodHabitInputModel>(ResponseCode.NotFound, "Not Found");
        }

        bool allowed = await repo.IsOwnerOf(id, userId);
        if(!allowed)
        {
            return new Response<GoodHabitInputModel>(ResponseCode.Unauthorized, "Unauthorized");
        }

        GoodHabitInputModel model = (await repo.GetInputModel(id))!;

        return new Response<GoodHabitInputModel>(ResponseCode.Success, "Success", model);
    }

    public async Task<Response<GoodHabitLogicModel>> GetLogicModel(int id, string userId)
    {

        bool exists = await repo.Exists(id);
        if(!exists)
        {
            return new Response<GoodHabitLogicModel>(ResponseCode.NotFound, "Not Found");
        }

        bool allowed = await repo.IsOwnerOf(id, userId);
        if(!allowed)
        {
            return new Response<GoodHabitLogicModel>(ResponseCode.Unauthorized, "Unauthorized");
        }

        GoodHabitLogicModel model = (await repo.GetLogicModel(id))!;

        return new Response<GoodHabitLogicModel>(ResponseCode.Success, "Success", model);
    }


    public async Task<Response> Update(int id, GoodHabitInputModel model, string userId)
    {
        if(model.Name.Length < GoodHabitNameLengthMin || model.Name.Length > GoodHabitNameLengthMax)
        {
            return new Response(ResponseCode.InvalidField, GoodHabitNameLengthError);
        }

        if(model.CreditsSuccess < GoodHabitCreditsSuccessMin || model.CreditsSuccess > GoodHabitCreditsSuccessMax)
        {
            return new Response(ResponseCode.InvalidField, GoodHabitNameCreditsSuccessError);
        }

        if(model.CreditsFail < GoodHabitCreditsFailMin || model.CreditsFail > GoodHabitCreditsFailMax)
        {
            return new Response(ResponseCode.InvalidField, GoodHabitNameCreditsFailError);
        }

        //Putting those checks here is bad for UX, but good for DB, and I'm a programmer, so figures. We should have client-side validation anyway, so it's not important.
        bool exists = await repo.Exists(id);
        if(!exists)
        {
            return new Response(ResponseCode.NotFound, "Not Found");
        }

        bool allowed = await repo.IsOwnerOf(id, userId);
        if(!allowed)
        {
            return new Response(ResponseCode.Unauthorized, "Unauthorized");
        }

        //the notify requries access to the new ids. It's less than ideal, but has to suffice.
        bool success = await repo.Update(id, model) && await unitOfWork.SaveChangesAsync();
        if(!success)
        {
            return new Response(ResponseCode.RepositoryError, "Something went wrong. Please try again.");
        }

        //notify -----
        var bigResponse = Response.AggregateErrors(await NotifyStatusChange(true /*shouldn't matter*/, userId));
        if(bigResponse.Code == ResponseCode.ServiceError)
        {
            //do nothing. The current architecture makes it hard to inform the user about errors from elsewhere in a general way.
        }
        await unitOfWork.SaveChangesAsync();

        return new Response(ResponseCode.Success, "Success");
    }

    public async Task<Response[]> NotifyStatusChange(bool isActive, string userId)
    {
        List<Response> list = new List<Response>();
        foreach(var sub in changeStatusSubscribers)
        {
            list.Add(await sub.NotifyWhenStatusChange(isActive, userId));
        }
        return list.ToArray();
    }

    public void SubscribeToStatusChange(IGoodHabitObserver observer)
    {
        if(observer == null)
        {
            throw new ArgumentNullException();
        }
        //should handle duplicates as a HashSet.
        changeStatusSubscribers.Add(observer);
    }

    public void UnsubscribeToStatusChange(IGoodHabitObserver observer)
    {
        if(observer == null)
        {
            throw new ArgumentNullException();
        }
        changeStatusSubscribers.Remove(observer);
    }

    public Task<bool> Exists(int id)
    {
        return repo.Exists(id);
    }

    public Task<bool> IsOwnerOf(int id, string userId)
    {
        return repo.IsOwnerOf(id, userId);
    }
}
