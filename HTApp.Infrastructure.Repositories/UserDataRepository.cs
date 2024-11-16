using HTApp.Core.Contracts;
using HTApp.Infrastructure.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using static HTApp.Core.Contracts.ApplicationInvariants; 

namespace HTApp.Infrastructure.Repositories;

public class UserDataRepository : IUserDataRepository<string>
{
    private ApplicationDbContext db;
    private ILogger logger;

    public UserDataRepository(ApplicationDbContext db, ILogger logger)
    {
        this.db = db;
        this.logger = logger;
    }

    public Task<UserDataDump?> GetAll(string userId)
    {
        return db.AppUsers
            .Where(u => u.Id == userId)
            .Select(u => new UserDataDump
            {
                Credits = u.Credits,
                RefundsPerSession = u.RefundsPerSession,
            })
            .FirstOrDefaultAsync();
    }

    public Task<int> GetCredits(string userId)
    {
        return db.AppUsers.Where(u => u.Id == userId)
            .Select(u => u.Credits)
            .FirstOrDefaultAsync();
    }

    public Task<byte> GetRefundsPerSession(string userId)
    {
        return db.AppUsers.Where(u => u.Id == userId)
            .Select(u => u.RefundsPerSession)
            .FirstOrDefaultAsync();
    }

    public async ValueTask<bool> SetCredits(string userId, int newValue)
    {
        if(!(newValue >= UserDataCreditsMin && newValue <= UserDataCreditsMax))
        {
            return false;
        }

        var user = await db.AppUsers.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null) return false;

        user.Credits = newValue;
        return true;
    }

    public async ValueTask<bool> SetRefundsPerSession(string userId, byte newValue)
    {
        if(!(newValue >= UserDataRefundsMin && newValue <= UserDataRefundsMax))
        {
            return false;
        }

        var user = await db.AppUsers.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null) return false;
        user.RefundsPerSession = newValue;
        return true;
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
