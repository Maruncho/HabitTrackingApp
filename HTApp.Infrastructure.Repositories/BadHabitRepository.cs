﻿using HTApp.Core.API;
using HTApp.Infrastructure.EntityModels;
using HTApp.Infrastructure.EntityModels.Core;
using Microsoft.EntityFrameworkCore;

namespace HTApp.Infrastructure.Repositories;

public class BadHabitRepository
    : RepositoryBaseSoftDelete<BadHabit, int>,
      IBadHabitRepository
{
    public BadHabitRepository(ApplicationDbContext db) : base(db)
    {
    }

    public Task<BadHabitModel[]> GetAll(string userId)
    {
        return GetAll()
            .Where(x => x.User.Id == userId)
            .Select(x => new BadHabitModel
            {
                Id = x.Id,
                Name = x.Name,
                CreditsSuccess = x.CreditsSuccess,
                CreditsFail = x.CreditsFail,
            })
            .ToArrayAsync();
    }

    public Task<int[]> GetAllIds(string userId)
    {
        return GetAll()
            .Where(x => x.UserId == userId)
            .Select(x => x.Id)
            .ToArrayAsync();   
    }

    public async Task<BadHabitLogicModel?> GetLogicModel(int id)
    {
        BadHabit? entity = await Get(id);

        if(entity is null)
        {
            return null;
        }

        var model = new BadHabitLogicModel
        {
            Id = entity.Id,
            CreditsSuccess = entity.CreditsSuccess,
            CreditsFail = entity.CreditsFail,
        };

        return model;
    }

    public async Task<BadHabitInputModel?> GetInputModel(int id)
    {
        BadHabit? entity = await Get(id);

        if(entity is null)
        {
            return null;
        }

        var model = new BadHabitInputModel
        {
            UserId = entity.UserId,
            Name = entity.Name,
            CreditsSuccess = entity.CreditsSuccess,
            CreditsFail = entity.CreditsFail,
        };

        return model;
    }

    public Task<bool> Add(BadHabitInputModel model)
    {
        BadHabit entity = new BadHabit
        {
            Name = model.Name,
            CreditsSuccess = model.CreditsSuccess,
            CreditsFail = model.CreditsFail,
            UserId = model.UserId,
            IsDeleted = false,
        };

        Add(entity);
        return Task.FromResult(true);
    }

    public async Task<bool> Update(int id, BadHabitInputModel model)
    {
        BadHabit? entity = await Get(id);

        if (entity is null)
        {
            return false;
        }

        entity.Name = model.Name;
        entity.CreditsSuccess = model.CreditsSuccess;
        entity.CreditsFail = model.CreditsFail;
        //entity.UserId = model.UserId; //leave it for now, too much potential chaos

        Update(entity);
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        BadHabit? entity = await Get(id);

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
