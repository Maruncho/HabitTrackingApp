using HTApp.Core.API;
using static HTApp.Core.API.ApplicationInvariants;

namespace HTApp.Core.Services;

public class TreatService : ITreatService
{
    ITreatRepository repo;
    IUnitOfWork unitOfWork;

    public TreatService(ITreatRepository repo, IUnitOfWork unitOfWork)
    {
        this.repo = repo;
        this.unitOfWork = unitOfWork;
    }

    public async ValueTask<Response> Add(TreatInputModel model, string userId)
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

        bool success = await repo.Add(model) && await unitOfWork.SaveChangesAsync();
        if(!success)
        {
            return new Response(ResponseCode.RepositoryError, "Something went wrong. Please try again.");
        }

        return new Response(ResponseCode.Success, "Success.");
    }

    public async ValueTask<Response> Delete(int id, string userId)
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

        return new Response(ResponseCode.Success, "Success");
    }

    public async ValueTask<Response<TreatModel[]>> GetAll(string userId)
    {
        return new Response<TreatModel[]>(ResponseCode.Success, "Success.", await repo.GetAll(userId));
    }

    public async ValueTask<Response<TreatInputModel>> GetInputModel(int id, string userId)
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

    public async ValueTask<Response> Update(int id, TreatInputModel model, string userId)
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

        bool success = await repo.Update(id, model) && await unitOfWork.SaveChangesAsync();
        if(!success)
        {
            return new Response(ResponseCode.RepositoryError, "Something went wrong. Please try again.");
        }

        return new Response(ResponseCode.Success, "Success.");
    }
}
