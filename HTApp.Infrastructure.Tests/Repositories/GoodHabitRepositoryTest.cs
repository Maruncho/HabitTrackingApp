using HTApp.Core.Contracts;
using HTApp.Infrastructure.EntityModels.Core;
using HTApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace HTApp.Infrastructure.Tests.Repositories;

internal class GoodHabitRepositoryTest : DbContextSetupBase
{
    [Test]
    public async Task GetAllHappyCase()
    {
        foreach (string userId in new string[] { user1.Id, user2.Id })
        {
            GoodHabitModel[] fromRepo = await GoodHabitRepository.GetAll(userId);

            var expected = DbGoodHabits
                .Where(x => x.UserId == userId && x.IsDeleted == false)
                .ToDictionary(x => x.Id);

            Assert.That(expected.Count, Is.EqualTo(fromRepo.Length));
            foreach(GoodHabitModel re in fromRepo)
            {
                var ex = expected[re.Id];

                bool result = ex.Id == re.Id && ex.Name == re.Name && ex.CreditsSuccess == re.CreditsSuccess && ex.CreditsFail == re.CreditsFail && ex.IsActive == re.IsActive;
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
            GoodHabitModel[] empty = await GoodHabitRepository.GetAll(userId);
            Assert.That(empty, Is.Empty);
        }
    }

    [Test]
    public async Task GetInputModelTestBasicallyEFCoreWrapper()
    {
        var ids = DbGoodHabits.Select(x => x.Id);
        foreach(var id in ids)
        {
            GoodHabit ex = DbGoodHabits.First(x => x.Id == id);
            GoodHabitInputModel? re = await GoodHabitRepository.GetInputModel(id);

            if(ex.IsDeleted == true)
            {
                Assert.That(re, Is.Null);
            }
            else
            {
                Assert.That(re, Is.Not.Null);
                bool result = ex.UserId == re.UserId && ex.Name == re.Name && ex.CreditsSuccess == re.CreditsSuccess && ex.CreditsFail == re.CreditsFail && ex.IsActive == re.IsActive;
                Assert.That(result, Is.True);
            }
        }
    }

    [Test]
    public async Task AddTestBasicallyEFCoreWrapper()
    {
        //hopefully random enough, so no one can accidentally match it in test refactoring.
        var name = Guid.NewGuid().ToString();
        name = name.Substring(0, Math.Min(name.Length, ApplicationInvariants.GoodHabitNameLengthMax));

        var model = new GoodHabitInputModel
        {
            Name = name,
            CreditsSuccess = ApplicationInvariants.GoodHabitCreditsSuccessMax,
            CreditsFail = ApplicationInvariants.GoodHabitCreditsFailMax,
            IsActive = true,
            UserId = user1.Id,
        };
        await GoodHabitRepository.Add(model);
        db.SaveChanges();
        GoodHabit? added = db.GoodHabits.FirstOrDefault(x => x.Name == name && x.UserId == user1.Id);

        Assert.That(added, Is.Not.Null);
    }

    [Test]
    public async Task UpdateTestBasicallyEFCoreWrapper()
    {
        var name = Guid.NewGuid().ToString();
        name = name.Substring(0, Math.Min(name.Length, ApplicationInvariants.GoodHabitNameLengthMax));

        GoodHabit something = DbGoodHabits.First();

        var model = new GoodHabitInputModel
        {
            Name = name,
            CreditsSuccess = ApplicationInvariants.GoodHabitCreditsSuccessMax,
            CreditsFail = ApplicationInvariants.GoodHabitCreditsFailMax,
            IsActive = true,
            UserId = something.UserId,
        };


        bool result = await GoodHabitRepository.Update(something.Id, model);
        Assert.True(result);

        db.SaveChanges();
        GoodHabit? updated = db.GoodHabits.FirstOrDefault(x => x.Name == name && x.UserId == something.UserId);

        Assert.That(updated, Is.Not.Null);
    }

    [Test]
    public async Task DeleteTestBasicallyEFCoreWrapper()
    {
        GoodHabit? something = db.GoodHabits.First();

        await GoodHabitRepository.Delete(something.Id);
        db.SaveChanges();

        something = db.GoodHabits.Find(something.Id);

        Assert.That(something!.IsDeleted, Is.True);
    }
}