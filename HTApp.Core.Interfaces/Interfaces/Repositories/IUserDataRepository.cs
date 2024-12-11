namespace HTApp.Core.API;

// This repository is meant to be more granular.
public interface IUserDataRepository
{
    public Task<UserDataDump?> GetEverything(string userId);

    public Task<int> GetCredits(string userId);
    public Task<byte> GetRefundsPerSession(string userId);

    public Task SetCredits(string userId, int newValue);
    public Task SetRefundsPerSession(string userId, byte newValue);
}
