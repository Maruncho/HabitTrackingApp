
namespace HTApp.Core.API;

public interface ITransactionRepository
    : ICommon_AddableOnly<TransactionInputModel, int>
{
    public Task<TransactionModel[]> GetAll(string userId);

    public Task<TransactionModel[]> GetAll(string userId, int pageCount, int pageNumber, int additionalEntries = 0, string filterTypeName = "");

    public Task<int> GetCount(string userId);

    public Task<string[]> GetUsedTypeNames(string userId);
}
