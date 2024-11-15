﻿namespace HTApp.Core.Contracts;

// No sync methods, only async. It's easy to add them whenever you would need them.

// ASSUMPTIONS:
// The caller checks for valid arguments.
// The callee (a IGenericRepository<>) is responsible for change tracking.
//      SaveChangesAsync() informs the caller whether the changes were successful.
public interface IGenericRepository<Entity, IdType> : IGenericRepositoryImmutable<Entity, IdType>
{
    public Task DeleteAsync(Entity entity);
    public Task UpdateAsync(Entity entity);
}
