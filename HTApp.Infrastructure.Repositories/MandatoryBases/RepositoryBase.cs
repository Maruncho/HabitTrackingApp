using HTApp.Infrastructure.EntityModels;

namespace HTApp.Infrastructure.Repositories;

public abstract class RepositoryBase<Entity, EntityId>
    : RepositoryImmutableBase<Entity, EntityId>
    where Entity : class
{

    private ApplicationDbContext db;

    protected RepositoryBase(ApplicationDbContext db)
        : base (db)
    {
        this.db = db;
    }

    protected void Delete(Entity entity)
    {
        db.Remove<Entity>(entity);
    }

    protected void Delete<T>(T entity) where T : class
    {
        db.Remove<T>(entity);
    }
}
