using HTApp.Core.API;
using HTApp.Infrastructure.EntityModels.Core;

namespace HTApp.Infrastructure.Tests.Repositories;

internal class TreatRepositoryTest : DbContextSetupBase
{
    [Test]
    public async Task GetAllHappyCase()
    {
        foreach (string userId in new string[] { user1.Id, user2.Id })
        {
            TreatModel[] fromRepo = await TreatRepository.GetAll(userId);
            var expected = DbTreats
                .Where(x => x.UserId == userId && x.IsDeleted == false)
                .ToDictionary(x => x.Id);

            Assert.That(expected.Count, Is.EqualTo(fromRepo.Length));
            foreach(TreatModel re in fromRepo)
            {
                var ex = expected[re.Id];

                bool result = ex.Id == re.Id && ex.Name == re.Name && ex.CreditsPrice == re.Price && ex.QuantityPerSession == re.QuantityPerSession;
                Assert.That(result, Is.True);
            }
        }
    }

    [Test]
    public async Task GetAllInvalidUserIdShouldBeEmptyAsync()
    {
        //maybe introducing randomness ???? Virtually impossible matches ????
        var userIds = Enumerable.Range(1, 10).Select(x => new Guid().ToString());
        foreach(var userId in userIds)
        {
            TreatModel[] empty = await TreatRepository.GetAll(userId);
            Assert.That(empty, Is.Empty);
        }
    }

    [Test]
    public async Task GetAllIdAndQuanitityPerSessionPairsHappyCase()
    {
        foreach (string userId in new string[] { user1.Id, user2.Id })
        {
            var fromRepo = await TreatRepository.GetAllIdAndQuantityPerSessionPairs(userId);
            var expected = DbTreats
                .Where(x => x.UserId == userId && x.IsDeleted == false)
                .ToDictionary(x => x.Id, x => x.QuantityPerSession);

            Assert.That(expected.Count, Is.EqualTo(fromRepo.Length));
            foreach((int id, byte quantity) in fromRepo)
            {
                var exQuantity = expected[id];

                bool result = exQuantity == quantity;
                Assert.That(result, Is.True);
            }
        }
    }

    [Test]
    public async Task GetAllYadaYadaPairsInvalidUserIdShouldBeEmptyAsync()
    {
        //maybe introducing randomness ???? Virtually impossible matches ????
        var userIds = Enumerable.Range(1, 10).Select(x => new Guid().ToString());
        foreach(var userId in userIds)
        {
            var empty = await TreatRepository.GetAllIdAndQuantityPerSessionPairs(userId);
            Assert.That(empty, Is.Empty);
        }
    }

    [Test]
    public async Task GetInputModelTestBasicallyEFCoreWrapper()
    {
        var ids = DbTreats.Select(x => x.Id);
        foreach(var id in ids)
        {
            Treat ex = DbTreats.First(x => x.Id == id);
            TreatInputModel? re = await TreatRepository.GetInputModel(id);

            if(ex.IsDeleted == true)
            {
                Assert.That(re, Is.Null);
            }
            else
            {
                Assert.That(re, Is.Not.Null);
                bool result = ex.UserId == re.UserId && ex.Name == re.Name && ex.CreditsPrice == re.Price && ex.QuantityPerSession == re.QuantityPerSession;
                Assert.That(result, Is.True);
            }
        }
    }

    [Test]
    public async Task GetLogicModelTestBasicallyEFCoreWrapper()
    {
        var ids = DbTreats.Select(x => x.Id);
        foreach(var id in ids)
        {
            Treat ex = DbTreats.First(x => x.Id == id);
            TreatLogicModel? re = await TreatRepository.GetLogicModel(id);

            if(ex.IsDeleted == true)
            {
                Assert.That(re, Is.Null);
            }
            else
            {
                Assert.That(re, Is.Not.Null);
                bool result = ex.Id == re.Id && ex.CreditsPrice == re.Price && ex.QuantityPerSession == re.UnitsPerSession;
                Assert.That(result, Is.True);
            }
        }
    }

    [Test]
    public async Task AddTestBasicallyEFCoreWrapper()
    {
        //hopefully random enough, so no one can accidentally match it in test refactoring.
        var name = Guid.NewGuid().ToString();
        name = name.Substring(0, Math.Min(name.Length, ApplicationInvariants.TreatNameLengthMax));

        var model = new TreatInputModel
        {
            Name = name,
            Price = ApplicationInvariants.TreatPriceMax,
            QuantityPerSession = ApplicationInvariants.TreatQuantityPerSessionMax,
            UserId = user1.Id,
        };
        await TreatRepository.Add(model);
        db.SaveChanges();
        Treat? added = db.Treats.FirstOrDefault(x => x.Name == name && x.UserId == user1.Id);

        Assert.That(added, Is.Not.Null);
    }

    [Test]
    public async Task UpdateTestBasicallyEFCoreWrapper()
    {
        var name = Guid.NewGuid().ToString();
        name = name.Substring(0, Math.Min(name.Length, ApplicationInvariants.TreatNameLengthMax));

        Treat something = DbTreats.First();

        var model = new TreatInputModel
        {
            Name = name,
            Price = ApplicationInvariants.TreatPriceMax,
            QuantityPerSession = ApplicationInvariants.TreatQuantityPerSessionMax,
            UserId = something.UserId,
        };


        bool result = await TreatRepository.Update(something.Id, model);
        Assert.True(result);

        db.SaveChanges();
        Treat? updated = db.Treats.FirstOrDefault(x => x.Name == name && x.UserId == something.UserId);

        Assert.That(updated, Is.Not.Null);

        //test when treatId is not found in db
        Assert.That(await TreatRepository.Update(-123, model), Is.False);
    }

    [Test]
    public async Task DeleteTestBasicallyEFCoreWrapper()
    {
        Treat? something = db.Treats.First();

        await TreatRepository.Delete(something.Id);
        db.SaveChanges();

        something = db.Treats.Find(something.Id);

        Assert.That(something!.IsDeleted, Is.True);

        //test when treatId is not found in db
        Assert.That(await TreatRepository.Delete(-123), Is.False);
    }

    [Test]
    public async Task ExistsTest()
    {
        var some = db.Treats.First();

        Assert.That(await TreatRepository.Exists(some.Id), Is.True);
        Assert.That(await TreatRepository.Exists(-123), Is.False);
    }
    
    [Test]
    public async Task IsOwnerOfTest()
    {
        var some = db.Treats.First();

        Assert.That(await TreatRepository.IsOwnerOf(some.Id, some.UserId), Is.True);
        Assert.That(await TreatRepository.IsOwnerOf(some.Id, some.UserId == user1.Id ? user2.Id : user1.Id), Is.False);
        Assert.That(await TreatRepository.IsOwnerOf(-123, some.UserId), Is.False);
    }
}
