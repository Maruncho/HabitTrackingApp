using HTApp.Core.API;

namespace HTApp.Core.Tests.Services.Implementations;

internal class BadTransactionRepository : ITransactionRepository
{
    public ValueTask<bool> Add(TransactionInputModel model)
    {
        return ValueTask.FromResult(true);
    }

    public ValueTask<bool> Exists(int id)
    {
        return ValueTask.FromResult(true);
    }

    public Task<TransactionModel[]> GetAll(string userId)
    {
        return Task.FromResult(Array.Empty<TransactionModel>());
    }

    public Task<TransactionModel[]> GetAll(string userId, int pageCount, int pageNumber, TransactionOptions? extra = null)
    {
        return Task.FromResult(Array.Empty<TransactionModel>());
    }

    public Task<int> GetCount(string userId, string filterTypeName = "", int? fromSessionId = null)
    {
        return Task.FromResult(1);
    }

    public Task<string[]> GetUsedTypeNames(string userId, string filterTypeName = "", int? fromSessionId = null)
    {
        return Task.FromResult(Array.Empty<string>());
    }

    public ValueTask<bool> IsOwnerOf(int id, string userId)
    {
        return ValueTask.FromResult(true);
    }
}
