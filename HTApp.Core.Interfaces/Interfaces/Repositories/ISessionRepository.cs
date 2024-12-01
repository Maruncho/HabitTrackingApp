namespace HTApp.Core.Contracts;

public interface ISessionRepository
{
    public Task<SessionModel?>
        GetCurrentSession(string userId);

    public Task<int?> GetCurrentSessionId(string userId);

    public Task<SessionUpdateModel?>
        GetCurrentSessionUpdateModel(string userId);

    public ValueTask<bool> AddAndMakeCurrent(SessionAddModel model);

    public ValueTask<bool> Update(SessionUpdateModel model);

    public ValueTask<bool> DeleteCurrentAndMakePreviousCurrent(string userId);
}
