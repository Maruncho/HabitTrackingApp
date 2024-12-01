namespace HTApp.Core.Contracts;

// This repository is meant to be more granular.
public interface IUserDataRepository
{
    public Task<UserDataDump?> GetEverything(string userId);

    public Task<int> GetCredits(string userId);
    public Task<byte> GetRefundsPerSession(string userId);

    public ValueTask SetCredits(string userId, int newValue);
    public ValueTask SetRefundsPerSession(string userId, byte newValue);
}
