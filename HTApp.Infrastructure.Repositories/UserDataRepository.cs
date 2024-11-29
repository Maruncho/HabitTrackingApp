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
}
