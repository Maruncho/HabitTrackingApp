using HTApp.Core.API;
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
            .Where(x => x.UserId == userId)
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

    public Task<int[]> GetAllIds(string userId, bool onlyActive)
    {
        var ids = GetAll().Where(x => x.UserId == userId);
        if(onlyActive)
        {
            ids = ids.Where(x => x.IsActive);
        }
        return ids
            .Select(x => x.Id)
            .ToArrayAsync();
    }

    public async Task<GoodHabitLogicModel?> GetLogicModel(int id)
    {
        GoodHabit? entity = await Get(id);

        if(entity is null)
        {
            return null;
        }

        var model = new GoodHabitLogicModel
        {
            Id = entity.Id,
            CreditsSuccess = entity.CreditsSuccess,
            CreditsFail = entity.CreditsFail,
            IsActive = entity.IsActive
        };

        return model;
    }

    public async Task<GoodHabitInputModel?> GetInputModel(int id)
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

    public Task<bool> Add(GoodHabitInputModel model)
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
        return Task.FromResult(true);
    }

    public async Task<bool> Update(int id, GoodHabitInputModel model)
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

    public async Task<bool> Delete(int id)
    {
        GoodHabit? entity = await Get(id);

        if (entity is null)
        {
            return false;
        }

        Delete(entity);
        return true;
    }
    public async Task<bool> IsOwnerOf(int id, string userId)
    {
        var model = await Get(id);
        return model is not null && model.UserId == userId;
    }

    public async Task<bool> Exists(int id)
    {
        return (await Get(id)) is not null;
    }
}
