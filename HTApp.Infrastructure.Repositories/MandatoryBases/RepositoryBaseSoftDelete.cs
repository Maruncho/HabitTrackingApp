﻿using HTApp.Infrastructure.EntityModels;

namespace HTApp.Infrastructure.Repositories;

public abstract class RepositoryBaseSoftDelete<Entity, IdType>
    : RepositoryImmutableBaseSoftDelete<Entity, IdType>
    where Entity : SoftDeletable
{

    private ApplicationDbContext db;

    protected RepositoryBaseSoftDelete(ApplicationDbContext db)
        : base (db)
    {
        this.db = db;
    }

    protected void Delete(Entity entity)
    {
        entity.IsDeleted = true; 
        db.Update(entity); //just in case
    }
}
