using HTApp.Core.API;
using HTApp.Infrastructure.EntityModels.Core;
using System.Transactions;
using Transaction = HTApp.Infrastructure.EntityModels.Core.Transaction;

namespace HTApp.Infrastructure.Tests.Repositories;

internal class TransactionRepositoryTest : DbContextSetupBase
{
    private static Dictionary<int, string> intToStringEnum =
        Enum.GetValues(typeof(TransactionEnum))
            .Cast<TransactionEnum>()
            .ToDictionary(t => (int)t, t => t.ToString());

    private static Dictionary<string, int> stringToIntEnum =
        Enum.GetValues(typeof(TransactionEnum))
            .Cast<TransactionEnum>()
            .ToDictionary(t => t.ToString(), t => (int)t);

    [Test]
    public async Task GetAllHappyCase()
    {
        foreach (string userId in new string[] { user1.Id, user2.Id })
        {
            TransactionModel[] fromRepo = await TransactionRepository.GetAll(userId);
            var expected = DbTransactions
                .Where(x => x.UserId == userId)
                .ToDictionary(x => x.Id);

            Assert.That(expected.Count, Is.EqualTo(fromRepo.Length));
            foreach(TransactionModel re in fromRepo)
            {
                var ex = expected[re.Id];

                bool result = ex.Id == re.Id && ex.Amount == re.Amount && ex.TypeId == stringToIntEnum[re.Type];
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
            TransactionModel[] empty = await TransactionRepository.GetAll(userId);
            Assert.That(empty, Is.Empty);
        }
    }

    [Test]
    public async Task GetAllWithParamsHappyCase()
    {
        foreach (var (userId, sId, filter) in new ValueTuple<string,int?,string>[]{
            (user1.Id, null, ""), (user2.Id, null, ""),
            (user1.Id, user1.Sessions.First().Id, ""), (user2.Id, user2.Sessions.First().Id, ""),
            (user1.Id, null, "Manual"), (user2.Id, null, "Manual")
        })
        {
            int pageCount = 5;
            int pageNumber = 1;

            Core.API.TransactionOptions? opts = new Core.API.TransactionOptions
            {
                AdditionalEntries = 0,
                FilterTypeName = filter,
                FromSessionId = sId,
            };
            if(sId is null && filter == "")
            {
                opts = null;
            }

            TransactionModel[] fromRepo = await TransactionRepository.GetAll(userId, pageCount, pageNumber, opts);
            var expected = DbTransactions
                .Where(x => x.UserId == userId)
                .Where(x => sId is null ? true : x.SessionId == sId)
                .Where(x => filter == "" ? true : x.TypeId == stringToIntEnum[filter])
                .OrderByDescending(x => x.Id)
                .Skip(pageCount * (pageNumber -1))
                .Take(pageCount)
                .ToDictionary(x => x.Id);

            Assert.That(expected.Count, Is.EqualTo(fromRepo.Length));
            foreach(TransactionModel re in fromRepo)
            {
                var ex = expected[re.Id];

                bool result = ex.Id == re.Id && ex.Amount == re.Amount && ex.TypeId == stringToIntEnum[re.Type];
                Assert.That(result, Is.True);
            }
        }
    }

    [Test]
    public async Task GetCountTest()
    {
        foreach (var (userId, sId, filter) in new ValueTuple<string,int?,string>[]{
            (user1.Id, null, ""), (user2.Id, null, ""),
            (user1.Id, user1.Sessions.First().Id, ""), (user2.Id, user2.Sessions.First().Id, ""),
            (user1.Id, null, "Manual"), (user2.Id, null, "Manual")
        })
        {
            int fromRepo = await TransactionRepository.GetCount(userId, filter, sId);
            var expected = DbTransactions
                .Where(x => x.UserId == userId)
                .Where(x => sId is null ? true : x.SessionId == sId)
                .Where(x => filter == "" ? true : x.TypeId == stringToIntEnum[filter])
                .Count();

            Assert.That(expected, Is.EqualTo(fromRepo));
        }
    }

    [Test]
    public async Task GetUsedTypesNamesTest()
    {
        foreach (var (userId, sId, filter) in new ValueTuple<string,int?,string>[]{
            (user1.Id, null, ""), (user2.Id, null, ""),
            (user1.Id, user1.Sessions.First().Id, ""), (user2.Id, user2.Sessions.First().Id, ""),
            (user1.Id, null, "Manual"), (user2.Id, null, "Manual")
        })
        {
            HashSet<string> expected = DbTransactions
                .Where(x => x.UserId == userId)
                .Where(x => sId is null ? true : x.SessionId == sId)
                .Where(x => filter == "" ? true : x.TypeId == stringToIntEnum[filter])
                .Select(x => intToStringEnum[x.TypeId]).ToHashSet();
            string[] fromRepo = await TransactionRepository.GetUsedTypeNames(userId, filter, sId);

            Assert.That(expected.Count, Is.EqualTo(fromRepo.Length));

            foreach (string x in fromRepo)
            {
                Assert.That(expected.TryGetValue(x, out _), Is.True);
            }
        }
    }

    [Test]
    public async Task AddTestBasicallyEFCoreWrapper()
    {
        var typeString = TransactionEnum.GoodHabitSuccess.ToString();
        var typeInt = (int)TransactionEnum.GoodHabitSuccess;
        var model = new TransactionInputModel
        {
            Amount = ApplicationInvariants.TransactionAmountMax,
            Type = typeString,
            UserId = user1.Id,
        };
        await TransactionRepository.Add(model);
        db.SaveChanges();
        Transaction? added = db.Transactions.FirstOrDefault(x => x.Amount == model.Amount && x.TypeId == typeInt);

        Assert.That(added, Is.Not.Null);
    }

    [Test]
    public async Task ExistsTest()
    {
        var some = db.Transactions.First();

        Assert.That(await TransactionRepository.Exists(some.Id), Is.True);
        Assert.That(await TransactionRepository.Exists(-123), Is.False);
    }
    
    [Test]
    public async Task IsOwnerOfTest()
    {
        var some = db.Transactions.First();

        Assert.That(await TransactionRepository.IsOwnerOf(some.Id, some.UserId), Is.True);
        Assert.That(await TransactionRepository.IsOwnerOf(some.Id, some.UserId == user1.Id ? user2.Id : user1.Id), Is.False);
        Assert.That(await TransactionRepository.IsOwnerOf(-123, some.UserId), Is.False);
    }
}
