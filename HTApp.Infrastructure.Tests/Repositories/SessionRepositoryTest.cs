using HTApp.Core.API;
using HTApp.Infrastructure.EntityModels.SessionModels;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

namespace HTApp.Infrastructure.Tests.Repositories;

class SessionRepositoryTest : DbContextSetupBase
{
    [Test]
    public async Task GetCurrentSessionTest()
    {
        foreach (var (userId, isNotFinished) in new ValueTuple<string, bool>[]
        { (user1.Id, false), (user2.Id, false),
          (user1.Id, true), (user2.Id, true)
        })
        {
            Session ex = DbSessions
                .Where(x => isNotFinished ? x.EndDate == null : true)
                .First(x => x.UserId == userId && x.Last);
            SessionModel? re = await SessionRepository.GetLastSession(userId, isNotFinished);

            Assert.That(re, Is.Not.Null);

            bool result = ex.Id == re.Id && ex.StartDate == re.StartDate && ex.EndDate == re.EndDate && ex.Refunds == re.Refunds;
            Assert.That(result, Is.True);

            Assert.That(ex.SessionGoodHabits.Count, Is.EqualTo(re.GoodHabits.Length));
            Assert.That(ex.SessionGoodHabits.All(x => re.GoodHabits.Any(y => x.GoodHabitId == y.Id)));
            Assert.That(ex.SessionBadHabits.Count, Is.EqualTo(re.BadHabits.Length));
            Assert.That(ex.SessionBadHabits.All(x => re.BadHabits.Any(y => x.BadHabitId == y.Id)));
            Assert.That(ex.SessionTreats.Count, Is.EqualTo(re.Treats.Length));
            Assert.That(ex.SessionTreats.All(x => re.Treats.Any(y => x.TreatId == y.Id)));
        }
    }

    [Test]
    public async Task GetLastSessionIdTest()
    {
        foreach (var userId in new string[] { user1.Id, user2.Id, new Guid().ToString() })
        {
            int? ex = DbSessions.FirstOrDefault(x => x.UserId == userId && x.Last)?.Id;
            int? re = await SessionRepository.GetLastSessionId(userId);

            Assert.That(ex, Is.EqualTo(re));
        }
    }

    [Test]
    public async Task GetIsLastSessionFinishedTest()
    {
        foreach(string userId in new string[] {user1.Id, user2.Id})
        {
            Session ex = DbSessions
                .First(x => x.UserId == userId && x.Last);
            var re = await SessionRepository.GetIsLastSessionFinished(userId);

            Assert.That(re, Is.Not.Null);

            bool result = ex.Id == re.Id && (ex.EndDate != null) == re.IsFinished;

            Assert.That(result, Is.True);
        }
    }

    [Test]
    public async Task GetJustRefundsTest()
    {
        foreach(string userId in new string[] {user1.Id, user2.Id})
        {
            byte ex = DbSessions
                .First(x => x.UserId == userId && x.Last).Refunds;
            var re = await SessionRepository.GetJustRefunds(userId);

            Assert.That(re, Is.Not.Null);

            Assert.That(ex, Is.EqualTo(re));
        }
    }

    [Test]
    public async Task DecrementRefundsTest()
    {
        foreach(string userId in new string[] {user1.Id, user2.Id, Guid.NewGuid().ToString() })
        {
            var result = await SessionRepository.DecrementRefunds(userId);

            var sessionOld = DbSessions.FirstOrDefault(x => x.UserId == userId && x.Last);
            if (sessionOld is null)
            { 
                Assert.That(result, Is.False);
                return;
            }

            await db.SaveChangesAsync();

            var sessionNew = db.Sessions.FirstOrDefault(x => x.UserId == userId && x.Last);

            Assert.That(sessionNew!.Refunds, Is.EqualTo(sessionOld!.Refunds - 1));
        }
    }

    [Test]
    public async Task GetGoodHabitIdsTest()
    {
        foreach(string userId in new string[] {user1.Id, user2.Id})
        {
           HashSet<int> ex = DbSessions.First(x => x.UserId == userId && x.Last)
                .SessionGoodHabits.Select(x => x.GoodHabitId).ToHashSet();
            int[]? re = await SessionRepository.GetGoodHabitIds(userId);

            Assert.That(re, Is.Not.Null);

            foreach(int id in ex)
            {
                Assert.That(ex.TryGetValue(id, out _), Is.True);
            }
        }
    }

    [Test]
    public async Task GetGoodHabitsFailedTest()
    {
        foreach(string userId in new string[] {user1.Id, user2.Id})
        {
            var expected = db.Sessions.First(x => x.UserId == userId && x.Last)
                .SessionGoodHabits.Where(gh => gh.Completed == false)
                .ToDictionary(x => x.GoodHabitId, x=> x.GoodHabit.CreditsFail);
            var fromRepo = await SessionRepository.GetGoodHabitsFailed(userId);

            Assert.That(fromRepo, Is.Not.Null);

            foreach(var re in fromRepo)
            {
                var exCredits = expected[re.Id];

                Assert.That(exCredits, Is.EqualTo(re.Credits));
            }
        }
    }

    [Test]
    public async Task GetGoodHabitCompletedTest()
    {
        foreach (var userId in new string[] { user1.Id, user2.Id })
        {
            var expected = db.Sessions.FirstOrDefault(x => x.UserId == userId && x.Last)?.SessionGoodHabits.ToArray();

            foreach(var ex in expected!)
            {
                var re = await SessionRepository.GetGoodHabitCompleted(ex.GoodHabitId, userId);

                Assert.That(re.HasValue, Is.True);

                Assert.That(re.Value, Is.EqualTo(ex.Completed));
            }
        }
    }

    [Test]
    public async Task UpdateGoodHabitTest()
    {
        foreach (var userId in new string[] { user1.Id, user2.Id })
        {
            var goodHabits = DbSessions.Where(x => x.UserId == userId && x.Last)
                .FirstOrDefault()?.SessionGoodHabits.ToArray();
            foreach(var gh in goodHabits ?? [])
            {
                await SessionRepository.UpdateGoodHabit(gh.GoodHabitId, !gh.Completed, userId);
                await db.SaveChangesAsync();

                var newGh = await db.SessionGoodHabits.FirstOrDefaultAsync(x => x.GoodHabitId == gh.GoodHabitId && x.SessionId == gh.SessionId);
                Assert.That(newGh, Is.Not.Null);
                Assert.That(newGh.Completed, Is.EqualTo(!gh.Completed));
            }
            //Test no session case
            Assert.That(await SessionRepository.UpdateGoodHabit(1, true, user3.Id), Is.False);
        }
    }
    
    [Test]
    public async Task GetBadHabitIdsTest()
    {
        foreach(string userId in new string[] {user1.Id, user2.Id})
        {
           HashSet<int> ex = DbSessions.First(x => x.UserId == userId && x.Last)
                .SessionBadHabits.Select(x => x.BadHabitId).ToHashSet();
            int[]? re = await SessionRepository.GetBadHabitIds(userId);

            Assert.That(re, Is.Not.Null);

            foreach(int id in ex)
            {
                Assert.That(ex.TryGetValue(id, out _), Is.True);
            }
        }
    }

    [Test]
    public async Task GetBadHabitsSuccessTest()
    {
        foreach(string userId in new string[] {user1.Id, user2.Id})
        {
            var expected = db.Sessions.First(x => x.UserId == userId && x.Last)
                .SessionBadHabits.Where(bh => bh.Failed == false)
                .ToDictionary(x => x.BadHabitId, x=> x.BadHabit.CreditsSuccess);
            var fromRepo = await SessionRepository.GetBadHabitsSuccess(userId);

            Assert.That(fromRepo, Is.Not.Null);

            foreach(var re in fromRepo)
            {
                var exCredits = expected[re.Id];

                Assert.That(exCredits, Is.EqualTo(re.Credits));
            }
        }
    }

    [Test]
    public async Task GetBadHabitFailedTest()
    {
        foreach (var userId in new string[] { user1.Id, user2.Id })
        {
            var expected = db.Sessions.FirstOrDefault(x => x.UserId == userId && x.Last)?.SessionBadHabits.ToArray();

            foreach(var ex in expected!)
            {
                var re = await SessionRepository.GetBadHabitFailed(ex.BadHabitId, userId);

                Assert.That(re.HasValue, Is.True);

                Assert.That(re.Value, Is.EqualTo(ex.Failed));
            }
        }
    }

    [Test]
    public async Task UpdateBadHabitTest()
    {
        foreach (var userId in new string[] { user1.Id, user2.Id })
        {
            var badHabits = DbSessions.Where(x => x.UserId == userId && x.Last)
                .FirstOrDefault()?.SessionBadHabits.ToArray();
            foreach(var bh in badHabits ?? [])
            {
                await SessionRepository.UpdateBadHabit(bh.BadHabitId, !bh.Failed, userId);
                await db.SaveChangesAsync();

                var newBh = await db.SessionBadHabits.FirstOrDefaultAsync(x => x.BadHabitId == bh.BadHabitId && x.SessionId == bh.SessionId);
                Assert.That(newBh, Is.Not.Null);
                Assert.That(newBh.Failed, Is.EqualTo(!bh.Failed));
            }
        }
        //Test no session case
        Assert.That(await SessionRepository.UpdateBadHabit(1, true, user3.Id), Is.False);
    }
    
    [Test]
    public async Task GetTreatIdsTest()
    {
        foreach(string userId in new string[] {user1.Id, user2.Id})
        {
           HashSet<int> ex = DbSessions.First(x => x.UserId == userId && x.Last)
                .SessionTreats.Select(x => x.TreatId).ToHashSet();
            int[]? re = await SessionRepository.GetTreatIds(userId);

            Assert.That(re, Is.Not.Null);

            foreach(int id in ex)
            {
                Assert.That(ex.TryGetValue(id, out _), Is.True);
            }
        }
    }

    [Test]
    public async Task GetTreatUnitsLeft()
    {
        foreach (var userId in new string[] { user1.Id, user2.Id })
        {
            var expected = db.Sessions.FirstOrDefault(x => x.UserId == userId && x.Last)?.SessionTreats.ToArray();

            foreach(var ex in expected!)
            {
                var re = await SessionRepository.GetTreatUnitsLeft(ex.TreatId, userId);

                Assert.That(re.HasValue, Is.True);

                Assert.That(re.Value, Is.EqualTo(ex.UnitsLeft));
            }
        }
    }

    [Test]
    public async Task DecrementTreatTest()
    {
        foreach(string userId in new string[] {user1.Id, user2.Id, Guid.NewGuid().ToString() })
        {
            var treat = DbSessions.FirstOrDefault(x => x.UserId == userId && x.Last)?.SessionTreats
                .FirstOrDefault();
            if (treat is null)
            { 
                var rsult = await SessionRepository.DecrementTreat(-1, userId);
                Assert.That(rsult, Is.False);
                return;
            }

            var result = await SessionRepository.DecrementTreat(treat.TreatId, userId);

            Assert.That(result, Is.True);

            await db.SaveChangesAsync();

            var treatNew = db.Sessions.FirstOrDefault(x => x.UserId == userId && x.Last)?.SessionTreats
                .FirstOrDefault(x => x.TreatId == treat.TreatId);

            Assert.That(treatNew, Is.Not.Null);

            Assert.That(treatNew.UnitsLeft, Is.EqualTo(treat!.UnitsLeft-1));
        }
    }

    [Test]
    public async Task IncrementTreatTest()
    {
        foreach(string userId in new string[] {user1.Id, user2.Id, Guid.NewGuid().ToString() })
        {
            var treat = DbSessions.FirstOrDefault(x => x.UserId == userId && x.Last)?.SessionTreats
                .FirstOrDefault();
            if (treat is null)
            { 
                var rsult = await SessionRepository.IncrementTreat(-1, userId);
                Assert.That(rsult, Is.False);
                return;
            }

            var result = await SessionRepository.IncrementTreat(treat.TreatId, userId);

            Assert.That(result, Is.True);

            await db.SaveChangesAsync();

            var treatNew = db.Sessions.FirstOrDefault(x => x.UserId == userId && x.Last)?.SessionTreats
                .FirstOrDefault(x => x.TreatId == treat.TreatId);

            Assert.That(treatNew, Is.Not.Null);

            Assert.That(treatNew.UnitsLeft, Is.EqualTo(treat!.UnitsLeft+1));
        }
    }

    [Test]
    public async Task StartNewSessionTest()
    {
        foreach (string userId in new string[] { user1.Id, user2.Id, user3.Id })
        {
            var ghs = DbGoodHabits.Where(x => x.IsActive && !x.IsDeleted && x.UserId == userId).ToArray();
            var bhs = DbBadHabits.Where(x => !x.IsDeleted && x.UserId == userId).ToArray();
            var trs = DbTreats.Where(x => !x.IsDeleted && x.UserId == userId).ToArray();

            var model = new SessionAddModel
            {
                StartDate = DateTime.Now,
                Refunds = 5,
                GoodHabitIds = ghs.Select(x => x.Id).ToArray(),
                BadHabitIds = bhs.Select(x => x.Id).ToArray(),
                TreatIdUnitPerSessionPairs = trs.Select(x => new Tuple<int, byte>(x.Id, x.QuantityPerSession)).ToArray(),
                UserId = userId
            };

            await SessionRepository.StartNewSession(model);
            await db.SaveChangesAsync();

            var oldSession = DbSessions.FirstOrDefault(x => x.UserId == userId && x.Last);

            var newSession = db.Sessions.FirstOrDefault(x => x.UserId == userId && x.Last);

            Assert.That(newSession!.PreviousSession?.Id, Is.EqualTo(oldSession?.Id));
            Assert.That(newSession!.Refunds, Is.EqualTo(5));
            Assert.That(newSession!.SessionGoodHabits.Count, Is.EqualTo(ghs.Count()));
            Assert.That(newSession!.SessionBadHabits.Count, Is.EqualTo(bhs.Count()));
            Assert.That(newSession!.SessionTreats.Count, Is.EqualTo(trs.Count()));
        }
    }

    [Test]
    public async Task FinishCurrentSessionTest()
    {
        foreach (string userId in new string[] { user1.Id, user2.Id, user3.Id })
        {
            var hasSession = DbSessions.Where(x => x.UserId == userId).Any();

            var result = await SessionRepository.FinishCurrentSession(userId);
            await db.SaveChangesAsync();

            if (!hasSession)
            {
                Assert.That(result, Is.False);
                return;
            }

            var updSession = db.Sessions.Where(x => x.UserId == userId && x.Last).FirstOrDefault()!;

            Assert.That(updSession.EndDate, Is.Not.Null);
        }
    }

    //[Test]
    //public async Task AddAndMakeCurrentTest()
    //{
    //    foreach (var userId in new string[] { user1.Id, user2.Id })
    //    {
    //        var dbLast = DbSessions.Where(x => x.UserId == userId && x.EndDate == null).First();
    //        var model = new SessionAddModel
    //        {
    //            StartDate = DateTime.Now,
    //            Refunds = 2,
    //            UserId = userId,
    //            PreviousSessionId = dbLast.Id,
    //            GoodHabitIds = dbLast.SessionGoodHabits.Select(x => x.GoodHabitId).ToArray(),
    //            BadHabitIds = dbLast.SessionBadHabits.Select(x => x.BadHabitId).ToArray(),
    //            TreatIdUnitPerSessionPairs = dbLast.SessionTreats.Select(x => new Tuple<int, byte>(x.TreatId, x.UnitsLeft)).ToArray(),
    //        };

    //        var res = await SessionRepository.AddAndMakeCurrent(model);
    //        Assert.That(res, Is.True);

    //        db.SaveChanges();

    //        var updatedFromDb = db.Sessions.Find(dbLast.Id)!;
    //        Assert.That(updatedFromDb.EndDate, Is.EqualTo(model.StartDate));

    //        var addedFromDb = db.Sessions.Where(x => x.UserId == userId && x.EndDate == null).FirstOrDefault();
    //        Assert.That(addedFromDb, Is.Not.Null);

    //        var re = addedFromDb;
    //        var ex = updatedFromDb;

    //        Assert.That(ex.SessionGoodHabits.Count, Is.EqualTo(re.SessionGoodHabits.Count));
    //        Assert.That(ex.SessionGoodHabits.All(x => re.SessionGoodHabits.Any(y => x.GoodHabitId == y.GoodHabitId)));
    //        Assert.That(ex.SessionBadHabits.Count, Is.EqualTo(re.SessionBadHabits.Count));
    //        Assert.That(ex.SessionBadHabits.All(x => re.SessionBadHabits.Any(y => x.BadHabitId == y.BadHabitId)));
    //        Assert.That(ex.SessionTreats.Count, Is.EqualTo(re.SessionTreats.Count));
    //        Assert.That(ex.SessionTreats.All(x => re.SessionTreats.Any(y => x.TreatId == y.TreatId)));
    //    }
    //}

    //[Test]
    //public async Task UpdateTest()
    //{
    //    foreach (string userId in new string[] { user1.Id, user2.Id })
    //    {
    //        //Should work. It's a shortcut, I know.
    //        var updateModel = (await SessionRepository.GetCurrentSessionUpdateModel(userId))!;
    //        updateModel.Refunds = 60;
    //        updateModel.GoodHabitIdCompletedPairs.Clear(); //Assuming it is already not. Yeah, right, some future maintainer, who's not me and not careful might change the mock data and this test won't work, but the chance of that happening is virtually zero, because every push request will go through me until it is made more general AND I TAKE FULL RESPONSIBILITY IF I F MYSELF OVER, WHICH IS NEVER GONNA HAPPEN IN THIS PROJECT ANYWAY.
    //        updateModel.BadHabitIdFailedPairs.Clear(); //same as above
    //        updateModel.TreatIdUnitsLeftPairs.Clear(); //same as above
    //        updateModel.TransactionIds.Clear(); // same as above
    //        var bh = DbBadHabits.First(x => x.Name == "h");
    //        updateModel.BadHabitIdFailedPairs.Add(bh.Id, true); // Assumed it's not been added before the clear(). same as above

    //        var res = await SessionRepository.Update(updateModel);
    //        Assert.That(res);
    //        db.SaveChanges();

    //        var updatedEntity = db.Sessions.Where(x => x.UserId == userId && x.EndDate == null).FirstOrDefault()!;

    //        Assert.That(updatedEntity.Refunds, Is.EqualTo(60));
    //        Assert.That(updatedEntity.SessionGoodHabits, Is.Empty);
    //        Assert.That(updatedEntity.SessionBadHabits.First().BadHabitId, Is.EqualTo(bh.Id));
    //        Assert.That(updatedEntity.SessionTreats, Is.Empty);
    //        Assert.That(updatedEntity.SessionTransactions, Is.Empty);
    //    }
    //}

    //[Test]
    //public async Task DeleteCurrentAndMakePreviousCurrent()
    //{
    //    foreach (string userId in new string[] { user1.Id, user2.Id })
    //    {
    //        var cur = db.Sessions.Where(x => x.UserId == userId && x.EndDate == null).Include(x => x.PreviousSession).First();
    //        var prev = cur.PreviousSession;

    //        var result = await SessionRepository.DeleteCurrentAndMakePreviousCurrent(userId);
    //        Assert.That(result, prev == null ? Is.False : Is.True);
    //        db.SaveChanges();

    //        if (prev is not null)
    //        {
    //            Assert.That(prev.Id, Is.EqualTo(await SessionRepository.GetCurrentSessionId(userId)));
    //        }
    //    }
    //}

    [Test]
    public async Task ExistsTest()
    {
        var some = db.Sessions.First();

        Assert.That(await SessionRepository.Exists(some.Id), Is.True);
        Assert.That(await SessionRepository.Exists(-123), Is.False);
    }
    
    [Test]
    public async Task IsOwnerOfTest()
    {
        var some = db.Sessions.First();

        Assert.That(await SessionRepository.IsOwnerOf(some.Id, some.UserId), Is.True);
        Assert.That(await SessionRepository.IsOwnerOf(some.Id, some.UserId == user1.Id ? user2.Id : user1.Id), Is.False);
        Assert.That(await SessionRepository.IsOwnerOf(-123, some.UserId), Is.False);
    }
}
