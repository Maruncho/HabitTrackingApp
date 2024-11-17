using HTApp.Core.Contracts;
using HTApp.Infrastructure.EntityModels;
using HTApp.Infrastructure.EntityModels.Core;
using Microsoft.EntityFrameworkCore;

namespace HTApp.Infrastructure.Repositories;

public class TreatRepository
    : RepositoryBase<Treat, int>,
      ITreatRepository<string, int, Treat>
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
    protected override async ValueTask<Treat?> Get(int id)
    {
        var res = await db.Treats.FindAsync(id);
        if (res?.IsDeleted ?? false) res = null;
        return res;
    }

    protected override IQueryable<Treat> GetAll()
    {
        return base.GetAll().Where(x => x.IsDeleted == false);
    }

    protected override void Delete(Treat entity)
    {
        entity.IsDeleted = true;
        db.Update(entity);
    }
}
