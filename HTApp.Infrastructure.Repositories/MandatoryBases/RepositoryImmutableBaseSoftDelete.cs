using HTApp.Infrastructure.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace HTApp.Infrastructure.Repositories;

public abstract class RepositoryImmutableBaseSoftDelete<Entity, EntityId>
    where Entity : SoftDeletable
{
    private ApplicationDbContext db;

    protected RepositoryImmutableBaseSoftDelete(ApplicationDbContext db)
    {
        this.db = db;
    }
    protected async ValueTask<Entity?> Get(EntityId id)
    {
        Entity? res = await db.FindAsync<Entity>(id);
        if (res is null) return null;
        return res.IsDeleted ? null : res;
    }

    protected IQueryable<Entity> GetAll()
    {
        return db.Set<Entity>()
            .Where(x => x.IsDeleted == false);
    }

    protected void Add(Entity entity)
    {
        db.Set<Entity>().Add(entity);
    }

    //pretty pointless, but maybe in the future it may do more.
    protected void Update(Entity entity)
    {
        //probably adds nothing
        db.Update(entity);
    }
}
