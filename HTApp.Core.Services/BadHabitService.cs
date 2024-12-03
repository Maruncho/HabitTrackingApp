using HTApp.Core.API;
using static HTApp.Core.API.ApplicationInvariants;

namespace HTApp.Core.Services;

public class BadHabitService : IBadHabitService
{
    IBadHabitRepository repo;
    IUnitOfWork unitOfWork;

    public BadHabitService(IBadHabitRepository repo, IUnitOfWork unitOfWork)
    {
        this.repo = repo;
        this.unitOfWork = unitOfWork;
    }

    public async ValueTask<Response> Add(BadHabitInputModel model, string userId)
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

    public async Task<Response<BadHabitModel[]>> GetAll(string userId)
    {
        return new Response<BadHabitModel[]>(ResponseCode.Success, "Success.", await repo.GetAll(userId));
    }

    public async ValueTask<Response<BadHabitInputModel>> GetInputModel(int id, string userId)
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

    public async ValueTask<Response> Update(int id, BadHabitInputModel model, string userId)
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

        bool success = await repo.Update(id, model) && await unitOfWork.SaveChangesAsync();
        if(!success)
        {
            return new Response(ResponseCode.RepositoryError, "Something went wrong. Please try again.");
        }

        return new Response(ResponseCode.Success, "Success.");
    }
}
