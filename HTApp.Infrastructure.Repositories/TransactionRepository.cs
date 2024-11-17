using HTApp.Core.Contracts;
using HTApp.Infrastructure.EntityModels;
using HTApp.Infrastructure.EntityModels.Core;
using Microsoft.EntityFrameworkCore;

namespace HTApp.Infrastructure.Repositories;

public class TransactionRepository
    : RepositoryImmutableBase<Transaction, int>
    , ITransactionRepository<string, int, Transaction>
{
    private Dictionary<int, string> enumConverter =
        Enum.GetValues(typeof(TransactionEnum))
           .Cast<TransactionEnum>()
           .ToDictionary(t => (int)t, t => t.ToString() );

    public TransactionRepository(ApplicationDbContext db) : base(db)
    {
    }

    public Task<TransactionModel[]> GetAll(string userId)
    {
        return db.Transactions
            .Where(t => t.UserId == userId)
            .Where(t => t.TypeId != null) //It's necessary for the enumConverter above. Shouldn't happen anyway, unless someone (me) screwed up.
            .Select(t => new TransactionModel
            {
                Id = t.Id,
                Type = enumConverter[t.TypeId!.Value],
                Message = t.Type!.Message ?? string.Empty,
                Amount = t.Amount
            })
            .ToArrayAsync();
    }

    public Task<string[]> GetTypeNames()
    {
        return db.Transactions
            .Where(t => t.TypeId != null)
            .Select(t => enumConverter[t.TypeId!.Value])
            .ToArrayAsync();
    }
}
