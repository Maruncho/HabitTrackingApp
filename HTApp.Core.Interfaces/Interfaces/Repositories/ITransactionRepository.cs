
namespace HTApp.Core.Contracts;

public interface ITransactionRepository<UserIdType, ModelIdType>
    : ICommon_AddableOnly<ModelIdType, TransactionInputModel<UserIdType>>
{
    public Task<TransactionModel[]> GetAll(UserIdType userId);

    public Task<string[]> GetTypeNames();
}
