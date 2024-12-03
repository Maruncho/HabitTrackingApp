
namespace HTApp.Core.API;

public interface ITransactionRepository
    : ICommon_AddableOnly<TransactionInputModel, int>
{
    public Task<TransactionModel[]> GetAll(string userId);

    public Task<string[]> GetUsedTypeNames();
}
