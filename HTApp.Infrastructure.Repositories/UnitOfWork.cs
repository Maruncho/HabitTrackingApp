using HTApp.Core.Contracts;
using HTApp.Infrastructure.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.Logging;

namespace HTApp.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork<string, int, int, int, int, int, int?>
{
    private ApplicationDbContext db;
    private ILogger logger;

    public IGoodHabitRepository<string, int> GoodHabitRepository { get; init; }
    public IBadHabitRepository<string, int> BadHabitRepository { get; init; }
    public ITreatRepository<string, int> TreatRepository { get; init; }
    public ITransactionRepository<string, int> TransactionRepository { get; init; }
    public ISessionRepository<string, int, int?, int, int, int, int> SessionRepository { get; init; }
    public IUserDataRepository<string> UserDataRepository { get; init; }

    public UnitOfWork(
        ApplicationDbContext db,
        ILogger logger,
        IGoodHabitRepository<string, int> gh,
        IBadHabitRepository<string, int> bh,
        ITreatRepository<string, int> tt,
        ITransactionRepository<string, int> ts,
        ISessionRepository<string, int, int?, int, int, int, int> se,
        IUserDataRepository<string> us
    )
    {
        this.db = db;
        this.logger = logger;
        GoodHabitRepository = gh;
        BadHabitRepository = bh;
        TreatRepository = tt;
        TransactionRepository = ts;
        SessionRepository = se;
        UserDataRepository = us;
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
