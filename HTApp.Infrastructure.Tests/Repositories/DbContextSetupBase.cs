using HTApp.Infrastructure.EntityModels;
using HTApp.Infrastructure.EntityModels.Core;
using HTApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HTApp.Infrastructure.Tests.Repositories;

internal class DbContextSetupBase
{
    public ApplicationDbContext db;

    public static AppUser user1;
    public static AppUser user2;

    public GoodHabit[] DbGoodHabits;
    public GoodHabitRepository GoodHabitRepository;


    [SetUp]
    public void Setup()
    {
        var config = new ConfigurationBuilder();
        config.AddUserSecrets(typeof(DbContextSetupBase).Assembly);
        var connectionString = config.Build().GetConnectionString("Testing");

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(connectionString) //In-memory-db is a toy, kinda, as an alternative that is.
            .Options;

        db = new ApplicationDbContext(options);

        //Adds a ~second to the duration of each test on my machine :(
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        db.AppUsers.Add(new AppUser());
        db.AppUsers.Add(new AppUser());
        db.SaveChanges();
        user1 = db.AppUsers.First();
        user2 = db.AppUsers.First(x => x.Id != user1.Id);   


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
        db.SaveChanges();
        DbGoodHabits = db.GoodHabits.ToArray();

        GoodHabitRepository = new GoodHabitRepository(db);
    }

    [TearDown]
    public void TearDown() {
        db.Dispose();
    }
}
