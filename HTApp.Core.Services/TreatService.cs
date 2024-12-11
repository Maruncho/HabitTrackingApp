using HTApp.Core.API;
using static HTApp.Core.API.ApplicationInvariants;

namespace HTApp.Core.Services;

public class TreatService : ITreatService
{
    ITreatRepository repo;
    IUnitOfWork unitOfWork;

    private HashSet<ITreatObserver> changeStatusSubscribers;

    public TreatService(ITreatRepository repo, IUnitOfWork unitOfWork)
    {
        this.repo = repo;
        this.unitOfWork = unitOfWork;

        changeStatusSubscribers = new();
    }

    public async Task<Response> Add(TreatInputModel model, string userId)
    {
        if(model.Name.Length < TreatNameLengthMin || model.Name.Length > TreatNameLengthMax)
        {
            return new Response(ResponseCode.InvalidField, TreatNameLengthError);
        }

        if(model.QuantityPerSession < TreatQuantityPerSessionMin || model.QuantityPerSession > TreatQuantityPerSessionMax)
        {
            return new Response(ResponseCode.InvalidField, TreatQuantityPerSessionError);
        }

        if(model.Price < TreatPriceMin || model.Price > TreatPriceMax)
        {
            return new Response(ResponseCode.InvalidField, TreatPriceError);
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

    public async Task<Response<TreatModel[]>> GetAll(string userId)
    {
        return new Response<TreatModel[]>(ResponseCode.Success, "Success.", await repo.GetAll(userId));
    }

    public async Task<Response<Tuple<int, byte>[]>> GetAllIdAndUnitsLeftPairs(string userId)
    {
        return new Response<Tuple<int, byte>[]>(ResponseCode.Success, "Success.", await repo.GetAllIdAndQuantityPerSessionPairs(userId));
    }

    public async Task<Response<TreatInputModel>> GetInputModel(int id, string userId)
    {
        bool exists = await repo.Exists(id);
        if(!exists)
        {
            return new Response<TreatInputModel>(ResponseCode.NotFound, "Not Found");
        }

        bool allowed = await repo.IsOwnerOf(id, userId);
        if(!allowed)
        {
            return new Response<TreatInputModel>(ResponseCode.Unauthorized, "Unauthorized");
        }

        TreatInputModel model = (await repo.GetInputModel(id))!;

        return new Response<TreatInputModel>(ResponseCode.Success, "Success", model);
    }
    public async Task<Response<TreatLogicModel>> GetLogicModel(int id, string userId)
    {
        bool exists = await repo.Exists(id);
        if(!exists)
        {
            return new Response<TreatLogicModel>(ResponseCode.NotFound, "Not Found");
        }

        bool allowed = await repo.IsOwnerOf(id, userId);
        if(!allowed)
        {
            return new Response<TreatLogicModel>(ResponseCode.Unauthorized, "Unauthorized");
        }

        TreatLogicModel model = (await repo.GetLogicModel(id))!;

        return new Response<TreatLogicModel>(ResponseCode.Success, "Success", model);
    }

    public async Task<Response> Update(int id, TreatInputModel model, string userId)
    {
        if(model.Name.Length < TreatNameLengthMin || model.Name.Length > TreatNameLengthMax)
        {
            return new Response(ResponseCode.InvalidField, TreatNameLengthError);
        }

        if(model.QuantityPerSession < TreatQuantityPerSessionMin || model.QuantityPerSession > TreatQuantityPerSessionMax)
        {
            return new Response(ResponseCode.InvalidField, TreatQuantityPerSessionError);
        }

        if(model.Price < TreatPriceMin || model.Price > TreatPriceMax)
        {
            return new Response(ResponseCode.InvalidField, TreatPriceError);
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

    public void SubscribeToStatusChange(ITreatObserver observer)
    {
        if(observer == null)
        {
            throw new ArgumentNullException();
        }
        //should handle duplicates as a HashSet.
        changeStatusSubscribers.Add(observer);
    }

    public void UnsubscribeToStatusChange(ITreatObserver observer)
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
