namespace HTApp.Core.API;

public interface ITransactionService
{
    public ValueTask<Response<TransactionServiceResponse>> GetAll(string userId, int pageCount, int pageNumber, string filterTypeName = "");
    public ValueTask<Response<TransactionServiceResponse>> GetAllLatest(string userId, int pageCount, string filterTypeName = "");
    public ValueTask<ResponseStruct<int>> GetCount(string userId);
    public ValueTask<Response<string[]>> GetTypeNames(string userId);
    public ValueTask<Response> Add(TransactionInputModel model, string userId);
    public ValueTask<Response> AddManual(int amount, string userId);
}
