using HTApp.Core.API;
using HTApp.Infrastructure.EntityModels.Core;

namespace HTApp.Infrastructure.Tests.Repositories;

internal class BadHabitRepositoryTest : DbContextSetupBase
{
    [Test]
    public async Task GetAllHappyCase()
    {
        foreach (string userId in new string[] { user1.Id, user2.Id })
        {
            BadHabitModel[] fromRepo = await BadHabitRepository.GetAll(userId);
            var expected = DbBadHabits
                .Where(x => x.UserId == userId && x.IsDeleted == false)
                .ToDictionary(x => x.Id);

            Assert.That(expected.Count, Is.EqualTo(fromRepo.Length));
            foreach(BadHabitModel re in fromRepo)
            {
                var ex = expected[re.Id];

                bool result = ex.Id == re.Id && ex.Name == re.Name && ex.CreditsSuccess == re.CreditsSuccess && ex.CreditsFail == re.CreditsFail;
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
            BadHabitModel[] empty = await BadHabitRepository.GetAll(userId);
            Assert.That(empty, Is.Empty);
        }
    }

    [Test]
    public async Task GetInputModelTestBasicallyEFCoreWrapper()
    {
        var ids = DbBadHabits.Select(x => x.Id);
        foreach(var id in ids)
        {
            BadHabit ex = DbBadHabits.First(x => x.Id == id);
            BadHabitInputModel? re = await BadHabitRepository.GetInputModel(id);

            if(ex.IsDeleted == true)
            {
                Assert.That(re, Is.Null);
            }
            else
            {
                Assert.That(re, Is.Not.Null);
                bool result = ex.UserId == re.UserId && ex.Name == re.Name && ex.CreditsSuccess == re.CreditsSuccess && ex.CreditsFail == re.CreditsFail;
                Assert.That(result, Is.True);
            }
        }
    }

    [Test]
    public async Task AddTestBasicallyEFCoreWrapper()
    {
        //hopefully random enough, so no one can accidentally match it in test refactoring.
        var name = Guid.NewGuid().ToString();
        name = name.Substring(0, Math.Min(name.Length, ApplicationInvariants.BadHabitNameLengthMax));

        var model = new BadHabitInputModel
        {
            Name = name,
            CreditsSuccess = ApplicationInvariants.BadHabitCreditsSuccessMax,
            CreditsFail = ApplicationInvariants.BadHabitCreditsFailMax,
            UserId = user1.Id,
        };
        await BadHabitRepository.Add(model);
        db.SaveChanges();
        BadHabit? added = db.BadHabits.FirstOrDefault(x => x.Name == name && x.UserId == user1.Id);

        Assert.That(added, Is.Not.Null);
    }

    [Test]
    public async Task UpdateTestBasicallyEFCoreWrapper()
    {
        var name = Guid.NewGuid().ToString();
        name = name.Substring(0, Math.Min(name.Length, ApplicationInvariants.BadHabitNameLengthMax));

        BadHabit something = DbBadHabits.First();

        var model = new BadHabitInputModel
        {
            Name = name,
            CreditsSuccess = ApplicationInvariants.BadHabitCreditsSuccessMax,
            CreditsFail = ApplicationInvariants.BadHabitCreditsFailMax,
            UserId = something.UserId,
        };


        bool result = await BadHabitRepository.Update(something.Id, model);
        Assert.True(result);

        db.SaveChanges();
        BadHabit? updated = db.BadHabits.FirstOrDefault(x => x.Name == name && x.UserId == something.UserId);

        Assert.That(updated, Is.Not.Null);
    }

    [Test]
    public async Task DeleteTestBasicallyEFCoreWrapper()
    {
        BadHabit? something = db.BadHabits.First();

        await BadHabitRepository.Delete(something.Id);
        db.SaveChanges();

        something = db.BadHabits.Find(something.Id);

        Assert.That(something!.IsDeleted, Is.True);
    }
}
