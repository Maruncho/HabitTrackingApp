using HTApp.Infrastructure.EntityModels;

namespace HTApp.Infrastructure.Repositories;

public abstract class RepositoryImmutableBaseSoftDelete<Entity, IdType>
    where Entity : SoftDeletable//, ISoftDeletable
{
    protected ApplicationDbContext db;

    protected RepositoryImmutableBaseSoftDelete(ApplicationDbContext db)
    {
        this.db = db;
    }
    public async ValueTask<Entity?> Get(int id)
    {
        Entity? res = await db.FindAsync<Entity>(id);
        if (res is null) return null;
        return res.IsDeleted ? null : res;
    }

    public IQueryable<Entity> GetAll()
    {
        return db.Set<Entity>()
            .Where(x => x.IsDeleted == false);
    }

}
