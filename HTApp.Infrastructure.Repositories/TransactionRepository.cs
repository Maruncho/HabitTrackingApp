using HTApp.Core.API;
using HTApp.Infrastructure.EntityModels;
using HTApp.Infrastructure.EntityModels.Core;
using Microsoft.EntityFrameworkCore;

namespace HTApp.Infrastructure.Repositories;

public class TransactionRepository
    : RepositoryImmutableBase<Transaction, int>
    , ITransactionRepository
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

    public Task<TransactionModel[]> GetAll(string userId)
    {
        return GetAll()
            .Where(t => t.UserId == userId)
            .Select(t => new TransactionModel
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

    public ValueTask<bool> Add(TransactionInputModel model)
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
    public async ValueTask<bool> IsOwnerOf(int id, string userId)
    {
        var model = await Get(id);
        return model is not null && model.UserId == userId;
    }

    public async ValueTask<bool> Exists(int id)
    {
        return (await Get(id)) is not null;
    }
}
