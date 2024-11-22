using HTApp.Core.Contracts;
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
            TransactionModel<int>[] fromRepo = await TransactionRepository.GetAll(userId);
            var expected = DbTransactions
                .Where(x => x.UserId == userId)
                .ToDictionary(x => x.Id);

            Assert.That(expected.Count, Is.EqualTo(fromRepo.Length));
            foreach(TransactionModel<int> re in fromRepo)
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
            TransactionModel<int>[] empty = await TransactionRepository.GetAll(userId);
            Assert.That(empty, Is.Empty);
        }
    }

    [Test]
    public async Task GetUsedTypesNamesTest()
    {
        HashSet<string> expected = DbTransactions.Select(x => intToStringEnum[x.TypeId]).ToHashSet();
        string[] fromRepo = await TransactionRepository.GetUsedTypeNames();

        Assert.That(expected.Count, Is.EqualTo(fromRepo.Length));

        foreach(string x in fromRepo)
        {
            Assert.That(expected.TryGetValue(x,out _), Is.True);
        }
    }

    [Test]
    public async Task AddTestBasicallyEFCoreWrapper()
    {
        var typeString = TransactionEnum.GoodHabitSuccess.ToString();
        var typeInt = (int)TransactionEnum.GoodHabitSuccess;
        var model = new TransactionInputModel<string>
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
}
