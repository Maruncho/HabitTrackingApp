using HTApp.Core.Contracts;
using HTApp.Infrastructure.EntityModels;
using HTApp.Infrastructure.EntityModels.Core;
using Microsoft.EntityFrameworkCore;

namespace HTApp.Infrastructure.Repositories;

public class BadHabitRepository
    : RepositoryBase<EntityModels.Core.BadHabit, int>,
      IBadHabitRepository<string, int, BadHabit>
{
    public BadHabitRepository(ApplicationDbContext db) : base(db)
    {
    }

    public Task<BadHabitModel[]> GetAll(string userId)
    {
        return db.BadHabits
            .Where(x => x.IsDeleted == false && x.User.Id == userId)
            .Select(x => new BadHabitModel
            {
                Id = x.Id,
                Name = x.Name,
                CreditsSuccess = x.CreditsSuccess,
                CreditsFail = x.CreditsFail,
            })
            .ToArrayAsync();
    }

    protected override async ValueTask<BadHabit?> Get(int id)
    {
        var res = await db.BadHabits.FindAsync(id);
        if (res?.IsDeleted ?? false) res = null;
        return res;
    }

    protected override ValueTask Delete(BadHabit entity)
    {
        entity.IsDeleted = true;
        db.Update(entity);
        return ValueTask.CompletedTask;
    }
}
