using HTApp.Core.Contracts;
using HTApp.Infrastructure.EntityModels;
using HTApp.Infrastructure.EntityModels.Core;
using Microsoft.EntityFrameworkCore;

namespace HTApp.Infrastructure.Repositories;

public class TreatRepository
    : RepositoryBaseSoftDelete<Treat, int>,
      ITreatRepository<string, int>
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

    public async ValueTask<TreatInputModel<string>?> GetInputModel(int id)
    {
        Treat? entity = await Get(id);

        if(entity is null)
        {
            return null;
        }

        var model = new TreatInputModel<string>
        {
            UserId = entity.UserId,
            Name = entity.Name,
            QuantityPerSession = entity.QuantityPerSession,
            Price = entity.CreditsPrice,
        };

        return model;
    }

    public ValueTask<bool> Add(TreatInputModel<string> model)
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

    public async ValueTask<bool> Update(int id, TreatInputModel<string> model)
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
}

