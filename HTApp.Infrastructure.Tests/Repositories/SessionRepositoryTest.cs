using HTApp.Core.Contracts;
using HTApp.Infrastructure.EntityModels.SessionModels;
using Microsoft.EntityFrameworkCore;

namespace HTApp.Infrastructure.Tests.Repositories;

class SessionRepositoryTest : DbContextSetupBase
{
    [Test]
    public async Task GetCurrentSessionTest()
    {
        foreach(var userId in new string[] {user1.Id, user2.Id})
        {
            Session ex = DbSessions.First(x => x.UserId == userId && x.EndDate == null);
            SessionModel? re = await SessionRepository.GetCurrentSession(userId);

            Assert.That(re, Is.Not.Null);

            bool result = ex.Id == re.Id && ex.StartDate == re.StartDate && ex.EndDate == re.EndDate && ex.Refunds == re.Refunds;
            Assert.That(result, Is.True);

            Assert.That(ex.SessionGoodHabits.Count, Is.EqualTo(re.GoodHabitIdCompletedPairs.Length));
            Assert.That(ex.SessionGoodHabits.All(x => re.GoodHabitIdCompletedPairs.Any(y => x.GoodHabitId == y.Item1)));
            Assert.That(ex.SessionBadHabits.Count, Is.EqualTo(re.BadHabitIdFailedPairs.Length));
            Assert.That(ex.SessionBadHabits.All(x => re.BadHabitIdFailedPairs.Any(y => x.BadHabitId == y.Item1)));
            Assert.That(ex.SessionTreats.Count, Is.EqualTo(re.TreatIdUnitsLeftPairs.Length));
            Assert.That(ex.SessionTreats.All(x => re.TreatIdUnitsLeftPairs.Any(y => x.TreatId == y.Item1)));
            Assert.That(ex.SessionTransactions.Count, Is.EqualTo(re.TransactionIds.Length));
            Assert.That(ex.SessionTransactions.All(x => re.TransactionIds.Any(y => x.TransactionId == y)));
        }
    }

    [Test]
    public async Task GetCurrentSessionIdTest()
    {

        foreach(var userId in new string[] {user1.Id, user2.Id, new Guid().ToString()})
        {
            int? ex = DbSessions.FirstOrDefault(x => x.UserId == userId && x.EndDate == null)?.Id;
            int? re = await SessionRepository.GetCurrentSessionId(userId);

            Assert.That(ex, Is.EqualTo(re));
        }
    }

    [Test]
    public async Task GetCurrentSessionUpdateModelTest()
    {
        foreach(var userId in new string[] {user1.Id, user2.Id})
        {
            Session ex = DbSessions.First(x => x.UserId == userId && x.EndDate == null);
            SessionUpdateModel? re = await SessionRepository.GetCurrentSessionUpdateModel(userId);

            Assert.That(re, Is.Not.Null);

            bool result = ex.Id == re.Id && ex.Refunds == re.Refunds && ex.UserId == re.UserId && ex.PreviousSessionId == re.PreviousSessionId;
            Assert.That(result, Is.True);

            Assert.That(ex.SessionGoodHabits.Count, Is.EqualTo(re.GoodHabitIdCompletedPairs.Count));
            Assert.That(ex.SessionGoodHabits.All(x => re.GoodHabitIdCompletedPairs.Any(y => x.GoodHabitId == y.Key)));
            Assert.That(ex.SessionBadHabits.Count, Is.EqualTo(re.BadHabitIdFailedPairs.Count));
            Assert.That(ex.SessionBadHabits.All(x => re.BadHabitIdFailedPairs.Any(y => x.BadHabitId == y.Key)));
            Assert.That(ex.SessionTreats.Count, Is.EqualTo(re.TreatIdUnitsLeftPairs.Count));
            Assert.That(ex.SessionTreats.All(x => re.TreatIdUnitsLeftPairs.Any(y => x.TreatId == y.Key)));
        }
    }

    [Test]
    public async Task AddAndMakeCurrentTest()
    {
        foreach (var userId in new string[] { user1.Id, user2.Id })
        {
            var dbLast = DbSessions.Where(x => x.UserId == userId && x.EndDate == null).First();
            var model = new SessionAddModel
            {
                StartDate = DateTime.Now,
                Refunds = 2,
                UserId = userId,
                PreviousSessionId = dbLast.Id,
                GoodHabitIds = dbLast.SessionGoodHabits.Select(x => x.GoodHabitId).ToArray(),
                BadHabitIds = dbLast.SessionBadHabits.Select(x => x.BadHabitId).ToArray(),
                TreatIdUnitPerSessionPairs = dbLast.SessionTreats.Select(x => new Tuple<int,byte>(x.TreatId, x.UnitsLeft)).ToArray(),
            };

            var res = await SessionRepository.AddAndMakeCurrent(model);
            Assert.That(res, Is.True);

            db.SaveChanges();

            var updatedFromDb = db.Sessions.Find(dbLast.Id)!;
            Assert.That(updatedFromDb.EndDate, Is.EqualTo(model.StartDate));

            var addedFromDb = db.Sessions.Where(x => x.UserId == userId && x.EndDate == null).FirstOrDefault();
            Assert.That(addedFromDb, Is.Not.Null);

            var re = addedFromDb;
            var ex = updatedFromDb;

            Assert.That(ex.SessionGoodHabits.Count, Is.EqualTo(re.SessionGoodHabits.Count));
            Assert.That(ex.SessionGoodHabits.All(x => re.SessionGoodHabits.Any(y => x.GoodHabitId == y.GoodHabitId)));
            Assert.That(ex.SessionBadHabits.Count, Is.EqualTo(re.SessionBadHabits.Count));
            Assert.That(ex.SessionBadHabits.All(x => re.SessionBadHabits.Any(y => x.BadHabitId == y.BadHabitId)));
            Assert.That(ex.SessionTreats.Count, Is.EqualTo(re.SessionTreats.Count));
            Assert.That(ex.SessionTreats.All(x => re.SessionTreats.Any(y => x.TreatId == y.TreatId)));
        }
    }

    [Test]
    public async Task UpdateTest()
    {
        foreach(string userId in new string[] {user1.Id, user2.Id})
        {
            //Should work. It's a shortcut, I know.
            var updateModel = (await SessionRepository.GetCurrentSessionUpdateModel(userId))!;
            updateModel.Refunds = 60;
            updateModel.GoodHabitIdCompletedPairs.Clear(); //Assuming it is already not. Yeah, right, some future maintainer, who's not me and not careful might change the mock data and this test won't work, but the chance of that happening is virtually zero, because every push request will go through me until it is made more general AND I TAKE FULL RESPONSIBILITY IF I F MYSELF OVER, WHICH IS NEVER GONNA HAPPEN IN THIS PROJECT ANYWAY.
            updateModel.BadHabitIdFailedPairs.Clear(); //same as above
            updateModel.TreatIdUnitsLeftPairs.Clear(); //same as above
            updateModel.TransactionIds.Clear(); // same as above
            var bh = DbBadHabits.First(x => x.Name == "h");
            updateModel.BadHabitIdFailedPairs.Add(bh.Id, true); // Assumed it's not been added before the clear(). same as above

            var res = await SessionRepository.Update(updateModel);
            Assert.That(res);
            db.SaveChanges();

            var updatedEntity = db.Sessions.Where(x => x.UserId == userId && x.EndDate == null).FirstOrDefault()!;

            Assert.That(updatedEntity.Refunds, Is.EqualTo(60));
            Assert.That(updatedEntity.SessionGoodHabits, Is.Empty);
            Assert.That(updatedEntity.SessionBadHabits.First().BadHabitId, Is.EqualTo(bh.Id));
            Assert.That(updatedEntity.SessionTreats, Is.Empty);
            Assert.That(updatedEntity.SessionTransactions, Is.Empty);
        }
    }

    [Test]
    public async Task DeleteCurrentAndMakePreviousCurrent()
    {
        foreach(string userId in new string[] {user1.Id, user2.Id })
        {
            var cur = db.Sessions.Where(x => x.UserId == userId && x.EndDate == null).Include(x => x.PreviousSession).First();
            var prev = cur.PreviousSession;

            var result = await SessionRepository.DeleteCurrentAndMakePreviousCurrent(userId);
            Assert.That(result, prev == null ? Is.False : Is.True);
            db.SaveChanges();

            if( prev is not null)
            {
                Assert.That(prev.Id, Is.EqualTo(await SessionRepository.GetCurrentSessionId(userId)));
            }
        }
    }
}
