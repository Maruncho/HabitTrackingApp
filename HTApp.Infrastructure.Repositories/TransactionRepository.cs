using HTApp.Core.Contracts;
using HTApp.Infrastructure.EntityModels;
using HTApp.Infrastructure.EntityModels.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HTApp.Infrastructure.Repositories
{
    using Transaction = EntityModels.Core.Transaction;

    public class TransactionRepository
        : GenericRepositoryImmutableBase<Transaction, int>
        , ITransactionRepository<string, int, Transaction>
    {
        private Dictionary<int, string> enumConverter =
            Enum.GetValues(typeof(TransactionEnum))
               .Cast<TransactionEnum>()
               .ToDictionary(t => (int)t, t => t.ToString() );

        public TransactionRepository(ApplicationDbContext db, ILogger logger) : base(db, logger)
        {
        }

        public Task<TransactionSimple[]> GetSimpleAll(string userId)
        {
            return db.Transactions
                .Where(t => t.UserId == userId)
                .Where(t => t.TypeId != null) //It's necessary for the enumConverter above. Shouldn't happen anyway, unless someone(me) screwed up.
                .Select(t => new TransactionSimple
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
}
