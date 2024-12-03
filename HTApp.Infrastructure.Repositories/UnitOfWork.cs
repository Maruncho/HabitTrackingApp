using HTApp.Core.API;
using HTApp.Infrastructure.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HTApp.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private ApplicationDbContext db;
    private ILogger<UnitOfWork> logger;

    public UnitOfWork(
        ApplicationDbContext db,
        ILogger<UnitOfWork> logger
    )
    {
        this.db = db;
        this.logger = logger;
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
            //I'm new to ASP.Net, so I don't know if there is a better way to log with more useful information.
            logger.LogError(e, "EF Core said this, trying to save:");
            return Task.FromResult(false);
        }
    }
}
