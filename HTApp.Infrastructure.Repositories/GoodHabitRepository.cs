using HTApp.Core.Contracts.Interfaces;
using HTApp.Infrastructure.EntityModels;
using HTApp.Infrastructure.EntityModels.Core;
using HTApp.Infrastructure.Repositories.Defaults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HTApp.Infrastructure.Repositories;

public class GoodHabitRepository(ApplicationDbContext db, ILogger logger)
    : GenericRepositoryBase<GoodHabit, int>(db, logger),
      IGoodHabitRepository<string, int, GoodHabit>
{
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
}
