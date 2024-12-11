using HTApp.Core.API;
using static HTApp.Core.API.ApplicationInvariants;

namespace HTApp.Core.Services;

public class BadHabitService : IBadHabitService
{
    IBadHabitRepository repo;
    IUnitOfWork unitOfWork;

    private HashSet<IBadHabitObserver> changeStatusSubscribers;

    public BadHabitService(IBadHabitRepository repo, IUnitOfWork unitOfWork)
    {
        this.repo = repo;
        this.unitOfWork = unitOfWork;

        changeStatusSubscribers = new();
    }

    public async Task<Response> Add(BadHabitInputModel model, string userId)
    {
        if(model.Name.Length < BadHabitNameLengthMin || model.Name.Length > BadHabitNameLengthMax)
        {
            return new Response(ResponseCode.InvalidField, BadHabitNameLengthError);
        }

        if(model.CreditsSuccess < BadHabitCreditsSuccessMin || model.CreditsSuccess > BadHabitCreditsSuccessMax)
        {
            return new Response(ResponseCode.InvalidField, BadHabitNameCreditsSuccessError);
        }

        if(model.CreditsFail < BadHabitCreditsFailMin || model.CreditsFail > BadHabitCreditsFailMax)
        {
            return new Response(ResponseCode.InvalidField, BadHabitNameCreditsFailError);
        }

        model.UserId = userId;

        //the notify requries access to the new ids. It's less than ideal, but has to suffice.
        bool success = await repo.Add(model) && await unitOfWork.SaveChangesAsync();
        if(!success)
        {
            return new Response(ResponseCode.RepositoryError, "Something went wrong. Please try again.");
        }

        //notify -----
        var bigResponse = Response.AggregateErrors(await NotifyStatusChange(userId));
        if(bigResponse.Code == ResponseCode.ServiceError)
        {
            //do nothing. The current architecture makes it hard to inform the user about errors from elsewhere in a general way.
        }
        await unitOfWork.SaveChangesAsync();

        return new Response(ResponseCode.Success, "Success.");
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

        //the notify requries access to the new ids. It's less than ideal, but has to suffice.
        bool success = await repo.Delete(id) && await unitOfWork.SaveChangesAsync();
        if(!success)
        {
            return new Response(ResponseCode.RepositoryError, "Something went wrong. Please try again.");
        }

        //notify -----
        var bigResponse = Response.AggregateErrors(await NotifyStatusChange(userId));
        if(bigResponse.Code == ResponseCode.ServiceError)
        {
            //do nothing. The current architecture makes it hard to inform the user about errors from elsewhere in a general way.
        }
        await unitOfWork.SaveChangesAsync();

        return new Response(ResponseCode.Success, "Success");
    }

    public async Task<Response<BadHabitModel[]>> GetAll(string userId)
    {
        return new Response<BadHabitModel[]>(ResponseCode.Success, "Success.", await repo.GetAll(userId));
    }
    public async Task<Response<int[]>> GetAllIds(string userId)
    {
        return new Response<int[]>(ResponseCode.Success, "Success.", await repo.GetAllIds(userId));
    }

    public async Task<Response<BadHabitInputModel>> GetInputModel(int id, string userId)
    {
        bool exists = await repo.Exists(id);
        if(!exists)
        {
            return new Response<BadHabitInputModel>(ResponseCode.NotFound, "Not Found");
        }

        bool allowed = await repo.IsOwnerOf(id, userId);
        if(!allowed)
        {
            return new Response<BadHabitInputModel>(ResponseCode.Unauthorized, "Unauthorized");
        }

        BadHabitInputModel model = (await repo.GetInputModel(id))!;

        return new Response<BadHabitInputModel>(ResponseCode.Success, "Success", model);
    }

    public async Task<Response<BadHabitLogicModel>> GetLogicModel(int id, string userId)
    {
        bool exists = await repo.Exists(id);
        if(!exists)
        {
            return new Response<BadHabitLogicModel>(ResponseCode.NotFound, "Not Found");
        }

        bool allowed = await repo.IsOwnerOf(id, userId);
        if(!allowed)
        {
            return new Response<BadHabitLogicModel>(ResponseCode.Unauthorized, "Unauthorized");
        }

        BadHabitLogicModel model = (await repo.GetLogicModel(id))!;

        return new Response<BadHabitLogicModel>(ResponseCode.Success, "Success", model);
    }

    public async Task<Response> Update(int id, BadHabitInputModel model, string userId)
    {
        if(model.Name.Length < BadHabitNameLengthMin || model.Name.Length > BadHabitNameLengthMax)
        {
            return new Response(ResponseCode.InvalidField, BadHabitNameLengthError);
        }

        if(model.CreditsSuccess < BadHabitCreditsSuccessMin || model.CreditsSuccess > BadHabitCreditsSuccessMax)
        {
            return new Response(ResponseCode.InvalidField, BadHabitNameCreditsSuccessError);
        }

        if(model.CreditsFail < BadHabitCreditsFailMin || model.CreditsFail > BadHabitCreditsFailMax)
        {
            return new Response(ResponseCode.InvalidField, BadHabitNameCreditsFailError);
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
        var bigResponse = Response.AggregateErrors(await NotifyStatusChange(userId));
        if(bigResponse.Code == ResponseCode.ServiceError)
        {
            //do nothing. The current architecture makes it hard to inform the user about errors from elsewhere in a general way.
        }
        await unitOfWork.SaveChangesAsync();

        return new Response(ResponseCode.Success, "Success.");
    }

    public async Task<Response[]> NotifyStatusChange(string userId)
    {
        List<Response> list = new List<Response>();
        foreach(var sub in changeStatusSubscribers)
        {
            list.Add(await sub.NotifyWhenStatusChange(userId));
        }
        return list.ToArray();
    }

    public void SubscribeToStatusChange(IBadHabitObserver observer)
    {
        if(observer == null)
        {
            throw new ArgumentNullException();
        }
        //should handle duplicates as a HashSet.
        changeStatusSubscribers.Add(observer);
    }

    public void UnsubscribeToStatusChange(IBadHabitObserver observer)
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
