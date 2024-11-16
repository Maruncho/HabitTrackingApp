using HTApp.Core.Contracts;
using HTApp.Infrastructure.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HTApp.Infrastructure.Repositories;

public abstract class RepositoryImmutableBase<Entity, IdType>
    where Entity : class
{
    protected ApplicationDbContext db;

    protected RepositoryImmutableBase(ApplicationDbContext db)
    {
        this.db = db;
    }

    protected virtual ValueTask<Entity?> Get(IdType id)
    {
        return db.FindAsync<Entity>(id);
    }

    protected virtual ValueTask Add(Entity entity)
    {
        db.Add<Entity>(entity);
        return ValueTask.CompletedTask;
    }
}
