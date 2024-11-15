namespace HTApp.Core.Contracts;

// No sync methods, only async. It's easy to add them whenever you would need them.

// ASSUMPTIONS:
// The caller checks for valid arguments.
// The callee (a IGenericRepository<>) is responsible for change tracking.
//      SaveChangesAsync() informs the caller whether the changes were successful.
public interface IGenericRepositoryImmutable<Entity, IdType>
{
    public ValueTask<Entity?> GetAsync(IdType id); //EF Core FindAsync returns ValueTask. Change if you don't use EF Core I guess.
    public Task AddAsync(Entity entity);
    public Task<bool> SaveChangesAsync();
}
