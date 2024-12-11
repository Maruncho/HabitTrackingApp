using HTApp.Core.API;
using HTApp.Infrastructure.EntityModels;
using HTApp.Infrastructure.EntityModels.Core;
using Microsoft.EntityFrameworkCore;

namespace HTApp.Infrastructure.Repositories;

public class TreatRepository
    : RepositoryBaseSoftDelete<Treat, int>,
      ITreatRepository
{
    public TreatRepository(ApplicationDbContext db) : base(db)
    {
    }

    public Task<TreatModel[]> GetAll(string userId)
    {
        return GetAll()
            .Where(x => x.User.Id == userId)
            .Select(x => new TreatModel
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.CreditsPrice,
                QuantityPerSession = x.QuantityPerSession,
            })
            .ToArrayAsync();
    }
    public Task<Tuple<int, byte>[]> GetAllIdAndQuantityPerSessionPairs(string userId)
    {
        return GetAll()
            .Where(x => x.UserId == userId)
            .Select(x => new Tuple<int ,byte>(x.Id, x.QuantityPerSession))
            .ToArrayAsync();   
    }

    public async Task<TreatLogicModel?> GetLogicModel(int id)
    {
        Treat? entity = await Get(id);

        if(entity is null)
        {
            return null;
        }

        var model = new TreatLogicModel
        {
            Id = entity.Id,
            UnitsPerSession = entity.QuantityPerSession,
            Price = entity.CreditsPrice
        };

        return model;
    }

    public async Task<TreatInputModel?> GetInputModel(int id)
    {
        Treat? entity = await Get(id);

        if(entity is null)
        {
            return null;
        }

        var model = new TreatInputModel
        {
            UserId = entity.UserId,
            Name = entity.Name,
            QuantityPerSession = entity.QuantityPerSession,
            Price = entity.CreditsPrice,
        };

        return model;
    }

    public Task<bool> Add(TreatInputModel model)
    {
        Treat entity = new Treat
        {
            Name = model.Name,
            QuantityPerSession = model.QuantityPerSession,
            CreditsPrice = model.Price,
            UserId = model.UserId,
            IsDeleted = false,
        };

        Add(entity);
        return Task.FromResult(true);
    }

    public async Task<bool> Update(int id, TreatInputModel model)
    {
        Treat? entity = await Get(id);

        if (entity is null)
        {
            return false;
        }

        entity.Name = model.Name;
        entity.QuantityPerSession = model.QuantityPerSession;
        entity.CreditsPrice = model.Price;

        Update(entity);
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        Treat? entity = await Get(id);

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

