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

    public async ValueTask<TreatInputModel?> GetInputModel(int id)
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

    public ValueTask<bool> Add(TreatInputModel model)
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
        return ValueTask.FromResult(true);
    }

    public async ValueTask<bool> Update(int id, TreatInputModel model)
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

    public async ValueTask<bool> Delete(int id)
    {
        Treat? entity = await Get(id);

        if (entity is null)
        {
            return false;
        }

        Delete(entity);
        return true;
    }

    public async ValueTask<bool> IsOwnerOf(int id, string userId)
    {
        var model = await Get(id);
        return model is not null && model.UserId == userId;
    }

    public async ValueTask<bool> Exists(int id)
    {
        return (await Get(id)) is not null;
    }
}

