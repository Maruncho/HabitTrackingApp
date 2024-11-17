﻿using HTApp.Infrastructure.EntityModels;

namespace HTApp.Infrastructure.Repositories;

public abstract class RepositoryImmutableBaseSoftDelete<Entity, IdType>
    where Entity : SoftDeletable
{
    private ApplicationDbContext db;

    protected RepositoryImmutableBaseSoftDelete(ApplicationDbContext db)
    {
        this.db = db;
    }
    protected async ValueTask<Entity?> Get(IdType id)
    {
        Entity? res = await db.FindAsync<Entity>(id);
        if (res is null) return null;
        return res.IsDeleted ? null : res;
    }

    protected IQueryable<Entity> GetAll()
    {
        return db.Set<Entity>()
            .Where(x => x.IsDeleted == false);
    }

}