namespace HTApp.Core.API;

public interface ITransactionService : ISessionObserver
{
    public Task<Response<TransactionServiceResponse>> GetAll(string userId, int pageCount, int pageNumber, string filterTypeName = "", bool fromLastSession = false);
    public Task<Response<TransactionServiceResponse>> GetAllLatest(string userId, int pageCount, string filterTypeName = "", bool fromLastSession = false);
    //public Task<ResponseStruct<int>> GetCount(string userId);
    public Task<Response<string[]>> GetTypeNames(string userId, string filterTypeName = "", bool fromLastSession = false);
    public Task<Response> Add(TransactionInputModel model, string userId, bool saveChanges = true);
    public Task<Response> AddManual(int amount, string userId);
}
