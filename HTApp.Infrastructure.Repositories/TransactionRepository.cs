using HTApp.Core.Contracts;
using HTApp.Infrastructure.EntityModels;
using HTApp.Infrastructure.EntityModels.Core;
using Microsoft.EntityFrameworkCore;

namespace HTApp.Infrastructure.Repositories;

public class TransactionRepository
    : RepositoryImmutableBase<Transaction, int>
    , ITransactionRepository<string, int>
{
    private static Dictionary<int, string> intToStringEnum =
        Enum.GetValues(typeof(TransactionEnum))
            .Cast<TransactionEnum>()
            .ToDictionary(t => (int)t, t => t.ToString());

    private static Dictionary<string, int> stringToIntEnum =
        Enum.GetValues(typeof(TransactionEnum))
            .Cast<TransactionEnum>()
            .ToDictionary(t => t.ToString(), t => (int)t);

    public TransactionRepository(ApplicationDbContext db) : base(db)
    {
    }

    public Task<TransactionModel<int>[]> GetAll(string userId)
    {
        return GetAll()
            .Where(t => t.UserId == userId)
            .Select(t => new TransactionModel<int>
            {
                Id = t.Id,
                Type = intToStringEnum[t.TypeId],
                Message = t.Type!.Message ?? string.Empty,
                Amount = t.Amount
            })
            .ToArrayAsync();
    }

    public Task<string[]> GetUsedTypeNames()
    {
        return GetAll()
            .Select(t => intToStringEnum[t.TypeId])
            .Distinct()
            .ToArrayAsync();
    }

    public ValueTask<bool> Add(TransactionInputModel<string> model)
    {
        Transaction entity = new Transaction
        {
            Amount = model.Amount,
            TypeId = stringToIntEnum[model.Type],
            UserId = model.UserId,
        };

        Add(entity);
        return ValueTask.FromResult(true);
    }
}
