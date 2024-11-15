using HTApp.Core.Contracts;
using HTApp.Infrastructure.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HTApp.Infrastructure.Repositories;

public abstract class GenericRepositoryBase<Entity, IdType>
    : GenericRepositoryImmutableBase<Entity, IdType>
    , IGenericRepository<Entity, IdType>
    where Entity : class
{
    protected GenericRepositoryBase(ApplicationDbContext db, ILogger logger)
        : base (db, logger)
    {
        this.db = db;
        this.logger = logger;
    }

    public virtual Task DeleteAsync(Entity entity)
    {
        db.Remove<Entity>(entity);
        return Task.CompletedTask;
    }

    public virtual Task UpdateAsync(Entity entity)
    {
        db.Update<Entity>(entity);
        return Task.CompletedTask;
    }
}
