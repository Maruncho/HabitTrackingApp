using HTApp.Core.Contracts;
using HTApp.Infrastructure.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HTApp.Infrastructure.Repositories;

public abstract class RepositoryBaseSoftDelete<Entity, IdType>
    : RepositoryImmutableBaseSoftDelete<Entity, IdType>
    where Entity : SoftDeletable
{
    protected RepositoryBaseSoftDelete(ApplicationDbContext db)
        : base (db)
    {
    }

    public virtual void Delete(Entity entity)
    {
        entity.IsDeleted = true; 
        db.Update(entity); //just in case
    }
}
