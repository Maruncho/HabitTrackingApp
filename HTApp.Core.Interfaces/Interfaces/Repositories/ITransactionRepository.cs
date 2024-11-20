
namespace HTApp.Core.Contracts;

public interface ITransactionRepository<UserIdType, ModelId>
    : ICommon_AddableOnly<ModelId, TransactionInputModel<UserIdType>>
{
    public Task<TransactionModel<ModelId>[]> GetAll(UserIdType userId);

    public Task<string[]> GetTypeNames();
}
