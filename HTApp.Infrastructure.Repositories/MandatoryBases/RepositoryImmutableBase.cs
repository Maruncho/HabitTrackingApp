using HTApp.Infrastructure.EntityModels;

namespace HTApp.Infrastructure.Repositories;

public abstract class RepositoryImmutableBase<Entity, IdType>
    where Entity : class
{
    private ApplicationDbContext db;

    protected RepositoryImmutableBase(ApplicationDbContext db)
    {
        this.db = db;
    }

    protected ValueTask<Entity?> Get(IdType id)
    {
        return db.FindAsync<Entity>(id);
    }

    protected IQueryable<Entity> GetAll()
    {
        return db.Set<Entity>();
    }
}
