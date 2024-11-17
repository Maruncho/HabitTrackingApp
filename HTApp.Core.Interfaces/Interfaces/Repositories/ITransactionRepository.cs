
namespace HTApp.Core.Contracts;

public interface ITransactionRepository<UserIdType, ModelIdType, SourceEntity>
    : ICommon_AddableOnly<ModelIdType, TransactionInputModel<UserIdType>>
{
    public Task<TransactionModel[]> GetAll(UserIdType userId);

    public Task<string[]> GetTypeNames();
}
