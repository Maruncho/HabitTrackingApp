using HTApp.Core.Contracts;
using HTApp.Infrastructure.EntityModels;
using HTApp.Infrastructure.EntityModels.Core;
using HTApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HTApp.Infrastructure.Tests.Repositories;

public class Tests
{

    private ApplicationDbContext db;
    private GoodHabit[] localData;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("name")
            .Options;

        db = new ApplicationDbContext(options);

        db.AppUsers.Add(new AppUser());
        db.SaveChanges();
        var user = db.AppUsers.First();

        localData =
        [
            new GoodHabit { Name = "a", CreditsSuccess = 0, CreditsFail = 0, User = user },
            new GoodHabit { Name = "b", CreditsSuccess = 0, CreditsFail = 0, User = user },
        ];
    }

    //It's all in one place, yes... but I really don't want to spend more time on this stuff. It was a painful learning experience.
    [Test]
    public async Task TestTheSoftDeleteFunctionalityBecauseItsSketchy()
    {
        db.GoodHabits.AddRange(localData);
        db.SaveChanges();

        var repo = new GoodHabitRepository(db);

        var data = repo.GetAll().ToArray();
        Assert.That(data[0].Name == localData[0].Name && data[0].IsDeleted == localData[0].IsDeleted);
        Assert.That(data[0].Name == localData[0].Name && data[0].IsDeleted == localData[0].IsDeleted);
        var dta = await repo.Get(1);
        Assert.That(dta.Name == localData[0].Name && dta.IsDeleted == localData[0].IsDeleted);

        repo.Delete(dta);
        dta = await repo.Get(1);
        Assert.Null(dta);
        dta = await repo.Get(2);
        Assert.That(dta.Name == localData[1].Name && dta.IsDeleted == localData[1].IsDeleted);
        repo.Delete(dta);

        db.SaveChanges(); //I forgot to do that and spent 30min clueless of what was going on...
        data = repo.GetAll().ToArray();
        Assert.That(data.Length == 0);
    }

    [TearDown]
    public void TearDown() {
        db.Dispose();
    }
}