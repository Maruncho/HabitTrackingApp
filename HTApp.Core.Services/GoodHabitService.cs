using HTApp.Core.API;
using static HTApp.Core.API.ApplicationInvariants;

namespace HTApp.Core.Services;

public class GoodHabitService : IGoodHabitService
{
    IGoodHabitRepository repo;
    IUnitOfWork unitOfWork;

    public GoodHabitService(IGoodHabitRepository repo, IUnitOfWork unitOfWork)
    {
        this.repo = repo;
        this.unitOfWork = unitOfWork;
    }

    public async ValueTask<Response> Add(GoodHabitInputModel model, string userId)
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

    public async ValueTask<Response<GoodHabitModel[]>> GetAll(string userId)
    {
        return new Response<GoodHabitModel[]>(ResponseCode.Success, "Success.", await repo.GetAll(userId));
    }

    public async ValueTask<Response<GoodHabitInputModel>> GetInputModel(int id, string userId)
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

    public async ValueTask<Response> Update(int id, GoodHabitInputModel model, string userId)
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

        bool success = await repo.Update(id, model) && await unitOfWork.SaveChangesAsync();
        if(!success)
        {
            return new Response(ResponseCode.RepositoryError, "Something went wrong. Please try again.");
        }

        return new Response(ResponseCode.Success, "Success.");
    }
}
