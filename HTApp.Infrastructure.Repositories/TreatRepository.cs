using HTApp.Core.Contracts;
using HTApp.Infrastructure.EntityModels;
using HTApp.Infrastructure.EntityModels.Core;
using Microsoft.EntityFrameworkCore;

namespace HTApp.Infrastructure.Repositories;

public class TreatRepository
    : RepositoryBaseSoftDelete<Treat, int>,
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

    public ValueTask Add(TreatModel model)
    {
        throw new NotImplementedException();
    }

    public ValueTask Update(int id, TreatModel model)
    {
        throw new NotImplementedException();
    }

    public ValueTask Delete(int id)
    {
        throw new NotImplementedException();
    }
}
