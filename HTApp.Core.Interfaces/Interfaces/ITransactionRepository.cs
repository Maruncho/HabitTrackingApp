
namespace HTApp.Core.Contracts;

// Side note: Don't you like inheritance + generics :)
public interface ITransactionRepository<UserIdType, ModelIdType, SourceEntity> : IGenericRepositoryImmutable<SourceEntity, ModelIdType>
{
    public Task<TransactionSimple[]> GetSimpleAll(UserIdType userId);

    public Task<string[]> GetTypeNames();
}
