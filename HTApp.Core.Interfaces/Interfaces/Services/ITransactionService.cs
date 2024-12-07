namespace HTApp.Core.API;

public interface ITransactionService : ISessionObserver
{
    public ValueTask<Response<TransactionServiceResponse>> GetAll(string userId, int pageCount, int pageNumber, string filterTypeName = "", bool fromLastSession = false);
    public ValueTask<Response<TransactionServiceResponse>> GetAllLatest(string userId, int pageCount, string filterTypeName = "", bool fromLastSession = false);
    public ValueTask<ResponseStruct<int>> GetCount(string userId);
    public ValueTask<Response<string[]>> GetTypeNames(string userId);
    public ValueTask<Response> Add(TransactionInputModel model, string userId, bool saveChanges = true);
    public ValueTask<Response> AddManual(int amount, string userId);
}
