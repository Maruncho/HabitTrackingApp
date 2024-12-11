namespace HTApp.Core.API;

public interface IUserDataService
{
    public Task<Response<UserDataDump>> GetUserData(string userId);
    public Task<ResponseStruct<int>> GetCredits(string userId);
    public Task<Response<AppendCreditsResponse>> AppendCredits(int credits, string userId, bool saveChanges = true /*kinda ad-hoc, but otherwise I have to split invariant checking from services*/);
    public Task<ResponseStruct<byte>> GetRefundsPerSession(string userId);
    public Task<Response> SetRefundsPerSession(byte newRefunds, string userId);
}
