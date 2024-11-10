using HTApp.Core.Contracts.Interfaces;
using HTApp.Infrastructure.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTApp.Infrastructure.Repositories.Defaults
{
    public abstract class GenericRepositoryBase<Entity, IdType>
        (ApplicationDbContext db, ILogger logger)
        : IGenericRepository<Entity, IdType>
        where Entity : class
    {
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
}
