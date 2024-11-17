using HTApp.Infrastructure.EntityModels;

namespace HTApp.Infrastructure.Repositories;

public abstract class RepositoryImmutableBaseSoftDelete<Entity, IdType>
    where Entity : class//, ISoftDeletable
{
    protected ApplicationDbContext db;

    protected RepositoryImmutableBaseSoftDelete(ApplicationDbContext db)
    {
        this.db = db;
    }
    protected async ValueTask<Entity?> Get(int id)
    {
        Entity? res = await db.FindAsync<Entity>(id);
        if (res is null) return res;
        var x = (ISoftDeletable) res; //🤨🤨🤨 It has to be done
        return x.IsDeleted ? res : null;
    }

    protected IQueryable<Entity> GetAll()
    {
        return db.Set<Entity>()
            .Where(x => ((ISoftDeletable)x).IsDeleted == false); //🤨🤨🤨 It has to be done
    }

}
