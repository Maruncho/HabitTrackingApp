using HTApp.Infrastructure.EntityModels;

namespace HTApp.Infrastructure.Repositories;

public abstract class RepositoryBase<Entity, IdType>
    : RepositoryImmutableBase<Entity, IdType>
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
}
