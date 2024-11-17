using HTApp.Core.Contracts;
using HTApp.Infrastructure.EntityModels;
using HTApp.Infrastructure.EntityModels.Core;
using Microsoft.EntityFrameworkCore;

namespace HTApp.Infrastructure.Repositories;

public class GoodHabitRepository
    : RepositoryBaseSoftDelete<GoodHabit, int>,
      IGoodHabitRepository<string, int, GoodHabit>
{
    public GoodHabitRepository(ApplicationDbContext db) : base(db)
    {
    }

    public Task<GoodHabitModel[]> GetAll(string userId)
    {
        return db.GoodHabits
            .Where(x => x.IsDeleted == false && x.User.Id == userId)
            .Select(x => new GoodHabitModel
            {
                Id = x.Id,
                Name = x.Name,
                CreditsSuccess = x.CreditsSuccess,
                CreditsFail = x.CreditsFail,
                IsActive = x.IsActive,
            })
            .ToArrayAsync();
    }
    //protected override async ValueTask<GoodHabit?> Get(int id)
    //{
    //    var res = await db.GoodHabits.FindAsync(id);
    //    if (res?.IsDeleted ?? false) res = null;
    //   return res;
    //}

    //protected override IQueryable<GoodHabit> GetAll()
    //{
    //    return base.GetAll().Where(x => x.IsDeleted == false);
    //}

    //protected override void Delete(GoodHabit entity)
    //{
    //    entity.IsDeleted = true;
    //    db.Update(entity);
    //}

    public Task<GoodHabitModel> Get(string userId)
    {
        throw new NotImplementedException();
    }

    public ValueTask Add(GoodHabitModel model)
    {
        throw new NotImplementedException();
    }

    public ValueTask Update(int id, GoodHabitModel model)
    {
        throw new NotImplementedException();
    }

    public ValueTask Delete(int id)
    {
        throw new NotImplementedException();
    }
}
