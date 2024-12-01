
namespace HTApp.Core.Contracts;

public interface ITransactionRepository
    : ICommon_AddableOnly<TransactionInputModel>
{
    public Task<TransactionModel[]> GetAll(string userId);

    public Task<string[]> GetUsedTypeNames();
}
