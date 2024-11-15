using HTApp.Core.Contracts;
using HTApp.Infrastructure.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HTApp.Infrastructure.Repositories;

public abstract class GenericRepositoryImmutableBase<Entity, IdType>
    : IGenericRepositoryImmutable<Entity, IdType>
    where Entity : class
{
    protected ApplicationDbContext db;
    protected ILogger logger;

    protected GenericRepositoryImmutableBase(ApplicationDbContext db, ILogger logger)
    {
        this.db = db;
        this.logger = logger;
    }

    public virtual ValueTask<Entity?> GetAsync(IdType id)
    {
        return db.FindAsync<Entity>(id);
    }

    public virtual Task AddAsync(Entity entity)
    {
        db.Add<Entity>(entity);
        return Task.CompletedTask;
    }

    public virtual Task<bool> SaveChangesAsync()
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
            //I'm new to ASP.Net, so I don't know if there is a better way to log with more useful information.
            logger.LogError(e, "EF Core said this, trying to save:");
            return Task.FromResult(false);
        }
    }
}
