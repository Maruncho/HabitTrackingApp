using HTApp.Infrastructure.EntityModels;
using HTApp.Infrastructure.EntityModels.Core;
using HTApp.Infrastructure.EntityModels.SessionModels;
using HTApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HTApp.Infrastructure.Tests.Repositories;

internal class DbContextSetupBase
{
    public ApplicationDbContext db;

    public static AppUser user1;
    public static AppUser user2;
    public UserDataRepository UserDataRepository;

    public GoodHabit[] DbGoodHabits;
    public GoodHabitRepository GoodHabitRepository;

    public BadHabit[] DbBadHabits;
    public BadHabitRepository BadHabitRepository;

    public Treat[] DbTreats;
    public TreatRepository TreatRepository;

    public TransactionType[] DbTransactionTypes;
    public Transaction[] DbTransactions;
    public TransactionRepository TransactionRepository;

    public Session[] DbSessions;
    public SessionRepository SessionRepository;

    public static ApplicationDbContext SetUpAndGetDbContext()
    {
        var config = new ConfigurationBuilder();
        config.AddUserSecrets(typeof(DbContextSetupBase).Assembly);
        var connectionString = config.Build().GetConnectionString("Testing");

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(connectionString) //In-memory-db is a toy, kinda, as an alternative that is.
            .Options;

        return new ApplicationDbContext(options);
    }

    [SetUp]
    public void Setup()
    {
        db = SetUpAndGetDbContext();

        //Adds a ~second to the duration of each test on my machine :(
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        db.AppUsers.Add(new AppUser() { Credits = 100, RefundsPerSession = 1});
        db.AppUsers.Add(new AppUser() { Credits = 200, RefundsPerSession = 2});
        db.SaveChanges();
        user1 = db.AppUsers.First();
        user2 = db.AppUsers.First(x => x.Id != user1.Id);

        UserDataRepository = new UserDataRepository(db);


        var localGoodHabits = new GoodHabit[]
        {
            new GoodHabit {Name = "a", CreditsSuccess = 100, CreditsFail = 10, IsActive = true, IsDeleted = false, User = user1},
            new GoodHabit {Name = "b", CreditsSuccess = 200, CreditsFail = 20, IsActive = false, IsDeleted = false, User = user1},
            new GoodHabit {Name = "c", CreditsSuccess = 300, CreditsFail = 30, IsActive = true, IsDeleted = true, User = user1},
            new GoodHabit {Name = "d", CreditsSuccess = 400, CreditsFail = 40, IsActive = false, IsDeleted = true, User = user1},
            new GoodHabit {Name = "e", CreditsSuccess = 500, CreditsFail = 50, IsActive = true, IsDeleted = false, User = user2},
            new GoodHabit {Name = "f", CreditsSuccess = 600, CreditsFail = 60, IsActive = false, IsDeleted = false, User = user2},
            new GoodHabit {Name = "g", CreditsSuccess = 700, CreditsFail = 70, IsActive = true, IsDeleted = true, User = user2},
            new GoodHabit {Name = "h", CreditsSuccess = 800, CreditsFail = 80, IsActive = false, IsDeleted = true, User = user2}
        };
        db.AddRange(localGoodHabits);

        var localBadHabits = new BadHabit[]
        {
            new BadHabit {Name = "a", CreditsSuccess = 100, CreditsFail = 10, IsDeleted = false, User = user1},
            new BadHabit {Name = "b", CreditsSuccess = 200, CreditsFail = 20, IsDeleted = false, User = user1},
            new BadHabit {Name = "c", CreditsSuccess = 300, CreditsFail = 30, IsDeleted = true, User = user1},
            new BadHabit {Name = "d", CreditsSuccess = 400, CreditsFail = 40, IsDeleted = true, User = user1},
            new BadHabit {Name = "e", CreditsSuccess = 500, CreditsFail = 50, IsDeleted = false, User = user2},
            new BadHabit {Name = "f", CreditsSuccess = 600, CreditsFail = 60, IsDeleted = false, User = user2},
            new BadHabit {Name = "g", CreditsSuccess = 700, CreditsFail = 70, IsDeleted = true, User = user2},
            new BadHabit {Name = "h", CreditsSuccess = 800, CreditsFail = 80, IsDeleted = true, User = user2}
        };
        db.AddRange(localBadHabits);

        var localTreats = new Treat[]
        {
            new Treat {Name = "a", CreditsPrice = 100, QuantityPerSession = 1, IsDeleted= false, User = user1},
            new Treat {Name = "b", CreditsPrice = 200, QuantityPerSession = 2, IsDeleted= false, User = user1},
            new Treat {Name = "c", CreditsPrice = 300, QuantityPerSession = 3, IsDeleted= true, User = user1},
            new Treat {Name = "d", CreditsPrice = 400, QuantityPerSession = 4, IsDeleted= true, User = user1},
            new Treat {Name = "e", CreditsPrice = 500, QuantityPerSession = 5, IsDeleted= false, User = user2},
            new Treat {Name = "f", CreditsPrice = 600, QuantityPerSession = 6, IsDeleted= false, User = user2},
            new Treat {Name = "g", CreditsPrice = 700, QuantityPerSession = 7, IsDeleted= true, User = user2},
            new Treat {Name = "h", CreditsPrice = 800, QuantityPerSession = 8, IsDeleted= true, User = user2},
        };
        db.AddRange(localTreats);

        DbTransactionTypes = db.TransactionTypes.ToArray();

        var localTransactions = new Transaction[]
        {
            new Transaction {Amount = 100, Type = DbTransactionTypes[0], User = user1},
            new Transaction {Amount = 200, Type = DbTransactionTypes[0], User = user1},
            new Transaction {Amount = 300, Type = DbTransactionTypes[1], User = user1},
            new Transaction {Amount = 400, Type = DbTransactionTypes[1], User = user1},
            new Transaction {Amount = 500, Type = DbTransactionTypes[2], User = user2},
            new Transaction {Amount = 600, Type = DbTransactionTypes[2], User = user2},
            new Transaction {Amount = 700, Type = DbTransactionTypes[3], User = user2},
            new Transaction {Amount = 800, Type = DbTransactionTypes[3], User = user2},
        };
        db.AddRange(localTransactions);

        var localSessions = new Session[]
        {
            new Session {StartDate = DateTime.Now, EndDate = DateTime.Now, Refunds = 1, User = user1 },
            new Session {StartDate = DateTime.Now, EndDate = DateTime.Now, Refunds = 2, User = user1 },
            new Session {StartDate = DateTime.Now, EndDate = null, Refunds = 3, User = user1,
                SessionGoodHabits = localGoodHabits.Where(x => x.User == user1 && x.IsActive && !x.IsDeleted).Select(x => new SessionGoodHabit{GoodHabit = x, Completed = false}).ToArray(),
                SessionBadHabits = localBadHabits.Where(x => x.User == user1 && !x.IsDeleted).Select(x => new SessionBadHabit{BadHabit = x, Failed = false}).ToArray(),
                SessionTreats = localTreats.Where(x => x.User == user1 && !x.IsDeleted).Select(x => new SessionTreat{Treat = x, UnitsLeft = 3}).ToArray(),
                SessionTransactions = localTransactions.Where(x => x.User == user1).Select(x => new SessionTransaction{Transaction = x}).ToArray(),
            },
            new Session {StartDate = DateTime.Now, EndDate = null, Refunds = 1, User = user2 },
        };
        db.AddRange(localSessions);


        db.SaveChanges();
        DbGoodHabits = db.GoodHabits.ToArray();
        DbBadHabits = db.BadHabits.ToArray();
        DbTreats = db.Treats.ToArray();
        DbTransactions = db.Transactions.ToArray();
        DbSessions = db.Sessions.Include(x => x.SessionGoodHabits).Include(x => x.SessionBadHabits).Include(x => x.SessionTransactions).Include(x => x.SessionTreats).ToArray();

        GoodHabitRepository = new GoodHabitRepository(db);
        BadHabitRepository = new BadHabitRepository(db);
        TreatRepository = new TreatRepository(db);
        TransactionRepository = new TransactionRepository(db);
        SessionRepository = new SessionRepository(db);
    }

    [TearDown]
    public void TearDown() {
        db.Dispose();
    }
}
