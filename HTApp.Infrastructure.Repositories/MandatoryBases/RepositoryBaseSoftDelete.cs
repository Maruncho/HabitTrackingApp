using HTApp.Infrastructure.EntityModels;

namespace HTApp.Infrastructure.Repositories;

public abstract class RepositoryBaseSoftDelete<Entity, EntityId>
    : RepositoryImmutableBaseSoftDelete<Entity, EntityId>
    where Entity : SoftDeletable
{

    private ApplicationDbContext db;

    protected RepositoryBaseSoftDelete(ApplicationDbContext db)
        : base (db)
    {
        this.db = db;
    }

    protected void Delete(Entity entity)
    {
        entity.IsDeleted = true; 
        db.Update(entity); //just in case
    }
}
