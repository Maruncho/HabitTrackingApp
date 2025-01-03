﻿using HTApp.Infrastructure.EntityModels;

namespace HTApp.Infrastructure.Repositories;

public abstract class RepositoryImmutableBase<Entity, EntityId>
    where Entity : class
{
    private ApplicationDbContext db;

    protected RepositoryImmutableBase(ApplicationDbContext db)
    {
        this.db = db;
    }

    protected ValueTask<Entity?> Get(EntityId id)
    {
        return db.Set<Entity>().FindAsync(id);
    }

    protected IQueryable<Entity> GetAll()
    {
        return db.Set<Entity>();
    }

    protected void Add(Entity entity)
    {
        db.Set<Entity>().Add(entity);
    }

    //pretty pointless, but maybe in the future it may do more.
    protected void Update(Entity entity)
    {
        //probably adds nothing
        db.Update(entity);
    }
}
