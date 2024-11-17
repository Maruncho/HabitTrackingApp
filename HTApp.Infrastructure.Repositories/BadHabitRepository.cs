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

    public Task<BadHabitModel> Get(string userId)
    {
        throw new NotImplementedException();
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

    public ValueTask Add(BadHabitModel model)
    {
        throw new NotImplementedException();
    }

    public ValueTask Update(int id, BadHabitModel model)
    {
        throw new NotImplementedException();
    }

    public ValueTask Delete(int id)
    {
        throw new NotImplementedException();
    }

    protected override void Delete(BadHabit entity)
    {
        entity.IsDeleted = true;
        db.Update(entity);
    }
}
