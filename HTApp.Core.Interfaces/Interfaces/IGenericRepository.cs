using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTApp.Core.Contracts.Interfaces;

// No sync methods, only async. It's easy to add them whenever you would need them.

// ASSUMPTIONS:
// The caller checks for valid arguments.
// The callee (a IGenericRepository<>) is responsible for change tracking.
//      SaveChangesAsync() informs the caller whether the changes were successful.
public interface IGenericRepository<Entity, IdType>
{
    public ValueTask<Entity?> GetAsync(IdType id); //EF Core FindAsync returns ValueTask. Change if you don't use EF Core I guess.
    public Task AddAsync(Entity entity);
    public Task DeleteAsync(Entity entity);
    public Task UpdateAsync(Entity entity);
    public Task<bool> SaveChangesAsync();
}
