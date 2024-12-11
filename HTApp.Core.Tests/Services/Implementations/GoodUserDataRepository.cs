using HTApp.Core.API;

namespace HTApp.Core.Tests.Services.Implementations;

internal class GoodUserDataRepository : IUserDataRepository
{
    public Task<int> GetCredits(string userId)
    {
        return Task.FromResult(999999);
    }

    public Task<UserDataDump?> GetEverything(string userId)
    {
        return Task.FromResult<UserDataDump?>(new UserDataDump());
    }

    public Task<byte> GetRefundsPerSession(string userId)
    {
        return Task.FromResult<byte>(100);
    }

    public ValueTask SetCredits(string userId, int newValue)
    {
        return ValueTask.CompletedTask;
    }

    public ValueTask SetRefundsPerSession(string userId, byte newValue)
    {
        return ValueTask.CompletedTask;
    }
}
