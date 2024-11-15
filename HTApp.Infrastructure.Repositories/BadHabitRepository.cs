using HTApp.Core.Contracts;
using HTApp.Infrastructure.EntityModels;
using HTApp.Infrastructure.EntityModels.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HTApp.Infrastructure.Repositories;

public class BadHabitRepository
    : GenericRepositoryBase<BadHabit, int>,
      IBadHabitRepository<string, int, BadHabit>
{
    public BadHabitRepository(ApplicationDbContext db, ILogger logger) : base(db, logger)
    {
    }

    public Task<BadHabitSimple[]> GetSimpleAll(string userId)
    {
        return db.BadHabits
            .Where(x => x.IsDeleted == false && x.User.Id == userId)
            .Select(x => new BadHabitSimple
            {
                Id = x.Id,
                Name = x.Name,
                CreditsSuccess = x.CreditsSuccess,
                CreditsFail = x.CreditsFail,
            })
            .ToArrayAsync();
    }

    public override async ValueTask<BadHabit?> GetAsync(int id)
    {
        var res = await db.BadHabits.FindAsync(id);
        if (res?.IsDeleted ?? false) res = null;
        return res;
    }

    public override Task DeleteAsync(BadHabit entity)
    {
        entity.IsDeleted = true;
        db.Update(entity);
        return Task.CompletedTask;
    }
}
