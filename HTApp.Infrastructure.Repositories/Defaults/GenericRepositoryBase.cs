using HTApp.Core.Contracts;
using HTApp.Infrastructure.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HTApp.Infrastructure.Repositories;

public abstract class GenericRepositoryBase<Entity, IdType>
    : IGenericRepository<Entity, IdType>
    where Entity : class
{
    protected ApplicationDbContext db;
    protected ILogger logger;

    protected GenericRepositoryBase(ApplicationDbContext db, ILogger logger)
    {
        this.db = db;
        this.logger = logger;
    }

    public ValueTask<Entity?> GetAsync(IdType id)
    {
        return db.FindAsync<Entity>(id);
    }

    public Task AddAsync(Entity entity)
    {
        db.Add<Entity>(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Entity entity)
    {
        db.Remove<Entity>(entity);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Entity entity)
    {
        db.Update<Entity>(entity);
        return Task.CompletedTask;
    }

    public Task<bool> SaveChangesAsync()
    {
        try
        {
            bool hasChanges = db.ChangeTracker.HasChanges();
            int res = db.SaveChanges();

            //Some simple quick check
            //if hasChanges == false -> true
            //else check if the SQL transaction from SaveChanges() saved something.
            return Task.FromResult(!hasChanges || res > 0); 
        }
        catch(DbUpdateException e)
        {
            logger.LogError(e, "EF Core said this, trying to save:");
            return Task.FromResult(false);
        }
    }
}
