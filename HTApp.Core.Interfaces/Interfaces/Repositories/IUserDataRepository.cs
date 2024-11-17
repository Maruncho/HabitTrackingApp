namespace HTApp.Core.Contracts;

// This repository is meant to be more granular.
public interface IUserDataRepository<UserIdType>
{
    public Task<UserDataDump?> GetEverything(UserIdType userId);

    public Task<int> GetCredits(UserIdType userId);
    public Task<byte> GetRefundsPerSession(UserIdType userId);

    public ValueTask SetCredits(UserIdType userId, int newValue);
    public ValueTask SetRefundsPerSession(UserIdType userId, byte newValue);
}
