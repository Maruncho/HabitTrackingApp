
namespace HTApp.Core.Contracts;

public interface ITransactionRepository<UserIdType, ModelIdType, SourceEntity>
{
    public Task<TransactionModel[]> GetAll(UserIdType userId);

    public Task<string[]> GetTypeNames();
}
