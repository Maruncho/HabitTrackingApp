
namespace HTApp.Core.API;

public interface ITransactionRepository
    : ICommon_AddableOnly<TransactionInputModel, int>
{
    public Task<TransactionModel[]> GetAll(string userId);

    public Task<TransactionModel[]> GetAll(string userId, int pageCount, int pageNumber, TransactionOptions? extra = null);

    public Task<int> GetCount(string userId, string filterTypeName = "", int? fromSessionId = null);

    public Task<string[]> GetUsedTypeNames(string userId);
}
