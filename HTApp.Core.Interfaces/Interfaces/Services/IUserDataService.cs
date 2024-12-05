namespace HTApp.Core.API;

public interface IUserDataService
{
    public ValueTask<Response<UserDataDump>> GetUserData(string userId);
    public ValueTask<ResponseStruct<int>> GetCredits(string userId);
    public ValueTask<Response<AppendCreditsResponse>> AppendCredits(int credits, string userId, bool saveChanges = true /*kinda ad-hoc, but otherwise I have to split invariant checking from services*/);
    public ValueTask<ResponseStruct<byte>> GetRefundsPerSession(string userId);
    public ValueTask<Response> SetRefundsPerSession(byte newRefunds, string userId);
}
