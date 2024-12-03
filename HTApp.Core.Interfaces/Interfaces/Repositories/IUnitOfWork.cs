namespace HTApp.Core.API;

// You can use the repositories only, if you wish so, but then the individual
// repository methods should do their own SaveChanges whenever needed. You still
// need to implement this interface, but leave the method implementations empty.
// EF Core works like a Unit of Work, so that's why I chose to use the pattern.
public interface IUnitOfWork
{
    public Task<bool> SaveChangesAsync();
}
