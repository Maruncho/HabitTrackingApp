using HTApp.Core.Contracts;
using HTApp.Infrastructure.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HTApp.Infrastructure.Repositories;

public abstract class RepositoryBase<Entity, IdType>
    : RepositoryImmutableBase<Entity, IdType>
    where Entity : class
{
    protected RepositoryBase(ApplicationDbContext db)
        : base (db)
    {
    }

    protected virtual ValueTask Delete(Entity entity)
    {
        db.Remove<Entity>(entity);
        return ValueTask.CompletedTask;
    }
}
