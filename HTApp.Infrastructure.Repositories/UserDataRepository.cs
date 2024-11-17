using HTApp.Core.Contracts;
using HTApp.Infrastructure.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace HTApp.Infrastructure.Repositories;

//This is an exception to the MandatoryBases
public class UserDataRepository : IUserDataRepository<string>
{
    private ApplicationDbContext db;

    public UserDataRepository(ApplicationDbContext db)
    {
        this.db = db;
    }

    public Task<UserDataDump?> GetEverything(string userId)
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
            .FirstAsync();
    }

    public Task<byte> GetRefundsPerSession(string userId)
    {
        return db.AppUsers.Where(u => u.Id == userId)
            .Select(u => u.RefundsPerSession)
            .FirstAsync();
    }

    public async ValueTask SetCredits(string userId, int newValue)
    {
        var user = await db.AppUsers.FindAsync(userId);
        user!.Credits = newValue;
    }

    public async ValueTask SetRefundsPerSession(string userId, byte newValue)
    {
        var user = await db.AppUsers.FindAsync(userId);
        user!.RefundsPerSession = newValue;
    }

    //public Task<bool> SaveChangesAsync()
    //{
    //    try
    //    {
    //        bool hasChanges = db.ChangeTracker.HasChanges();
    //        int res = db.SaveChanges();

    //        //Some simple quick check
    //        //if hasChanges == false -> true
    //        //else check if the SQL transaction from SaveChanges() saved something.
    //        return Task.FromResult(!hasChanges || res > 0); 
    //    }
    //    catch(DbUpdateException e)
    //    {
    //        //I'm new to ASP.Net, so I don't know if there is a better way to log with more useful information.
    //        logger.LogError(e, "EF Core said this, trying to save:");
    //        return Task.FromResult(false);
    //    }
    //}
}
