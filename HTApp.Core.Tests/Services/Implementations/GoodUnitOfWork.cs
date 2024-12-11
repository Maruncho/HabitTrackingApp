using HTApp.Core.API;

namespace HTApp.Core.Tests.Services.Implementations
{
    internal class GoodUnitOfWork : IUnitOfWork
    {
        public Task<bool> SaveChangesAsync()
        {
            return Task.FromResult(true);
        }
    }
}
