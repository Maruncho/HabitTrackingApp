using HTApp.Core.Contracts;
using HTApp.Infrastructure.EntityModels;
using HTApp.Infrastructure.EntityModels.Core;
using Microsoft.EntityFrameworkCore;

namespace HTApp.Infrastructure.Repositories;

public class GoodHabitRepository
    : RepositoryBaseSoftDelete<GoodHabit, int>,
      IGoodHabitRepository
{
    public GoodHabitRepository(ApplicationDbContext db) : base(db)
    {
    }

    public Task<GoodHabitModel[]> GetAll(string userId)
    {
        return GetAll()
            .Where(x => x.User.Id == userId)
            .Select(x => new GoodHabitModel
            {
                Id = x.Id,
                Name = x.Name,
                CreditsSuccess = x.CreditsSuccess,
                CreditsFail = x.CreditsFail,
                IsActive = x.IsActive,
            })
            .ToArrayAsync();
    }

    public async ValueTask<GoodHabitInputModel?> GetInputModel(int id)
    {
        GoodHabit? entity = await Get(id);

        if(entity is null)
        {
            return null;
        }

        var model = new GoodHabitInputModel
        {
            UserId = entity.UserId,
            Name = entity.Name,
            CreditsSuccess = entity.CreditsSuccess,
            CreditsFail = entity.CreditsFail,
            IsActive = entity.IsActive,
        };

        return model;
    }

    public ValueTask<bool> Add(GoodHabitInputModel model)
    {
        GoodHabit entity = new GoodHabit
        {
            Name = model.Name,
            CreditsSuccess = model.CreditsSuccess,
            CreditsFail = model.CreditsFail,
            IsActive = model.IsActive,
            UserId = model.UserId,
            IsDeleted = false,
        };

        Add(entity);
        return ValueTask.FromResult(true);
    }

    public async ValueTask<bool> Update(int id, GoodHabitInputModel model)
    {
        GoodHabit? entity = await Get(id);

        if (entity is null)
        {
            return false;
        }

        entity.Name = model.Name;
        entity.CreditsSuccess = model.CreditsSuccess;
        entity.CreditsFail = model.CreditsFail;
        entity.IsActive = model.IsActive;
        //entity.UserId = model.UserId; //leave it for now, too much potential chaos

        Update(entity);
        return true;
    }

    public async ValueTask<bool> Delete(int id)
    {
        GoodHabit? entity = await Get(id);

        if (entity is null)
        {
            return false;
        }

        Delete(entity);
        return true;
    }
}
