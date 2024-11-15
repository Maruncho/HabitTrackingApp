using HTApp.Core.Contracts;
using HTApp.Infrastructure.EntityModels;
using HTApp.Infrastructure.EntityModels.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.Logging;

namespace HTApp.Infrastructure.Repositories;

public class GoodHabitRepository
    : GenericRepositoryBase<GoodHabit, int>,
      IGoodHabitRepository<string, int, GoodHabit>
{
    public GoodHabitRepository(ApplicationDbContext db, ILogger logger) : base(db, logger)
    {
    }

    public Task<GoodHabitSimple[]> GetSimpleAll(string userId)
    {
        return db.GoodHabits
            .Where(x => x.IsDeleted == false && x.User.Id == userId)
            .Select(x => new GoodHabitSimple
            {
                Id = x.Id,
                Name = x.Name,
                CreditsSuccess = x.CreditsSuccess,
                CreditsFail = x.CreditsFail,
                IsActive = x.IsActive,
            })
            .ToArrayAsync();
    }
    public override async ValueTask<GoodHabit?> GetAsync(int id)
    {
        var res = await db.GoodHabits.FindAsync(id);
        if (res?.IsDeleted ?? false) res = null;
        return res;
    }

    public override Task DeleteAsync(GoodHabit entity)
    {
        entity.IsDeleted = true;
        db.Update(entity);
        return Task.CompletedTask;
    }
}
