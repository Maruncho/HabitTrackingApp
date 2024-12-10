using HTApp.Core.API;
using HTApp.Core.API.Models.RepoModels;
using HTApp.Infrastructure.EntityModels;
using HTApp.Infrastructure.EntityModels.SessionModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace HTApp.Infrastructure.Repositories;

public class SessionRepository
    : RepositoryBase<Session, int>,
      ISessionRepository
{

    public SessionRepository(ApplicationDbContext db) : base(db)
    {
    }

    public Task<SessionModel?> GetLastSession(string userId, bool isNotFinished)
    {
        var model = GetAllLatest(userId);

        if(isNotFinished)
        {
            model = model.Where(x => x.EndDate == null);
        }

        return model.Select(x => new SessionModel
            {
                Id = x.Id,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Refunds = x.Refunds,
                GoodHabits = x.SessionGoodHabits
                    .Select(gh => new SessionGoodHabitModel
                    {
                        Id = gh.GoodHabitId,
                        Label = gh.GoodHabit.Name,
                        Completed = gh.Completed
                    }).ToArray(),
                BadHabits = x.SessionBadHabits
                    .Select(bh => new SessionBadHabitModel
                    {
                        Id = bh.BadHabitId,
                        Label = bh.BadHabit.Name,
                        Failed = bh.Failed
                    }).ToArray(),
                Treats = x.SessionTreats
                    .Select(tr => new SessionTreatModel
                    {
                        Id = tr.TreatId,
                        Label = tr.Treat.Name,
                        UnitsLeft = tr.UnitsLeft,
                        UnitsBought = (byte)(tr.Treat.QuantityPerSession - tr.UnitsLeft),
                        Price = tr.Treat.CreditsPrice
                    }).ToArray()
            })
            .FirstOrDefaultAsync();
    }

    public Task<int?> GetLastSessionId(string userId)
    {
        return GetAllLatest(userId).Select(x => (int?)/*make nullable*/x.Id ).FirstOrDefaultAsync();
    }

    public Task<SessionIsFinishedModel?> GetIsLastSessionFinished(string userId)
    {
        return GetAllLatest(userId).Select(x => new SessionIsFinishedModel{ Id = x.Id, IsFinished = x.EndDate != null }).FirstOrDefaultAsync();
    }

    public Task<byte?> GetJustRefunds(string userId)
    {
        return GetAllLatest(userId).Select(x => (byte?)/*make nullable*/x.Refunds).FirstOrDefaultAsync();
    }

    public async Task<bool> DecrementRefunds(string userId)
    {
        Session? session = await GetLatest(userId);
        if(session is null)
        {
            return false;
        }

        session.Refunds = (byte)(session.Refunds - 1);
        Update(session);

        return true;
    }

    public Task<int[]?> GetGoodHabitIds(string userId)
    {
        return GetAllLatest(userId)
            .Select(x => x.SessionGoodHabits
                .Select(gh => gh.GoodHabitId)
                .ToArray()
            ).FirstOrDefaultAsync();
    }
    public Task<SessionHabitCreditsModel[]?> GetGoodHabitsFailed(string userId)
    {
        return GetAllLatest(userId)
            .Select(x => x.SessionGoodHabits
                .Where(gh => gh.Completed == false)
                .Select(gh => new SessionHabitCreditsModel
                {
                    Id = gh.GoodHabitId,
                    Credits = gh.GoodHabit.CreditsFail,
                }).ToArray()
            ).FirstOrDefaultAsync();
    }

    public async Task<bool?> GetGoodHabitCompleted(int id, string userId)
    {
        return (await GetAllLatest(userId)
            .Select(x => x.SessionGoodHabits
                .FirstOrDefault(gh => gh.GoodHabitId == id))
            .FirstOrDefaultAsync())?.Completed;
    }

    public async Task<UpdateInfo> UpdateGoodHabits(int[] ids, string userId)
    {
        Session? session = await GetLatest(userId);
        if (session is null)
        {
            return new UpdateInfo { Success = false };
        }

        var updateInfo = await updateGoodHabits(ids, (await GetGoodHabitIds(userId))!, session);

        Update(session);
        return updateInfo;
    }

    public async Task<bool> UpdateGoodHabit(int id, bool success, string userId)
    {
        Session? session = await GetAllLatest(userId)
            .Include(x => x.SessionGoodHabits
                .Where(gh => gh.GoodHabitId == id))
            .FirstOrDefaultAsync();

        if(session is null || session.SessionGoodHabits.Count == 0)
        {
            return false;
        }

        //not checked for null, kind of sloppy, but it's not meant to be validated and it's an exception for the dev to handle.
        session.SessionGoodHabits.First(x => x.GoodHabitId == id).Completed = success;
        Update(session);
        return true;
    }

    public Task<int[]?> GetBadHabitIds(string userId)
    {
        return GetAllLatest(userId)
            .Select(x => x.SessionBadHabits
                .Select(bh => bh.BadHabitId)
                .ToArray()
            ).FirstOrDefaultAsync();
    }
    public Task<SessionHabitCreditsModel[]?> GetBadHabitsSuccess(string userId)
    {
        return GetAllLatest(userId)
            .Select(x => x.SessionBadHabits
                .Where(bh => bh.Failed == false)
                .Select(gh => new SessionHabitCreditsModel
                {
                    Id = gh.BadHabitId,
                    Credits = gh.BadHabit.CreditsSuccess,
                }).ToArray()
            ).FirstOrDefaultAsync();
    }

    public async Task<bool?> GetBadHabitFailed(int id, string userId)
    {
        return (await GetAllLatest(userId)
            .Select(x => x.SessionBadHabits
                .FirstOrDefault(bh => bh.BadHabitId == id))
            .FirstOrDefaultAsync())?.Failed;
    }

    public async Task<UpdateInfo> UpdateBadHabits(int[] ids, string userId)
    {
        Session? session = await GetLatest(userId);
        if (session is null)
        {
            return new UpdateInfo { Success = false };
        }

        var updateInfo = await updateBadHabits(ids, (await GetBadHabitIds(userId))!, session);

        Update(session);
        return updateInfo;
    }

    public async Task<bool> UpdateBadHabit(int id, bool fail, string userId)
    {
        Session? session = await GetAllLatest(userId)
            .Include(x => x.SessionBadHabits
                .Where(bh => bh.BadHabitId == id))
            .FirstOrDefaultAsync();

        if(session is null || session.SessionBadHabits.Count == 0)
        {
            return false;
        }

        //not checked for null, kind of sloppy, but it's not meant to be validated and it's an exception for the dev to handle.
        session.SessionBadHabits.First(x => x.BadHabitId == id).Failed = fail;
        Update(session);
        return true;
    }

    public Task<int[]?> GetTreatIds(string userId)
    {
        return GetAllLatest(userId)
            .Select(x => x.SessionTreats
                .Select(tr => tr.TreatId)
                .ToArray()
            ).FirstOrDefaultAsync();
    }

    public async Task<byte?> GetTreatUnitsLeft(int id, string userId)
    {
        return (await GetAllLatest(userId)
            .Select(x => x.SessionTreats
                .FirstOrDefault(tr => tr.TreatId == id))
            .FirstOrDefaultAsync())?.UnitsLeft;
    }

    public async Task<UpdateInfo> UpdateTreats(Tuple<int, byte>[] idsAndUnitsPerSession, string userId)
    {
        Session? session = await GetLatest(userId);
        if (session is null)
        {
            return new UpdateInfo { Success = false };
        }

        var updateInfo = await updateTreats(idsAndUnitsPerSession, (await GetTreatIds(userId))!, session);

        Update(session);
        return updateInfo;
    }

    public async Task<bool> DecrementTreat(int id, string userId)
    {
        Session? session = await GetAllLatest(userId)
            .Include(x => x.SessionTreats
                .Where(tr => tr.TreatId == id))
            .FirstOrDefaultAsync();

        if(session is null || session.SessionTreats.Count == 0 )
        {
            return false;
        }

        //not checked for null, kind of sloppy, but it's not meant to be validated and it's an exception for the dev to handle.
        session.SessionTreats.First().UnitsLeft -= 1;
        Update(session);
        return true;
    }
    public async Task<bool> IncrementTreat(int id, string userId)
    {
        Session? session = await GetAllLatest(userId)
            .Include(x => x.SessionTreats
                .Where(tr => tr.TreatId == id))
            .FirstOrDefaultAsync();

        if(session is null || session.SessionGoodHabits.Count == 0)
        {
            return false;
        }

        //not checked for null, kind of sloppy, but it's not meant to be validated and it's an exception for the dev to handle.
        session.SessionTreats.First().UnitsLeft += 1;
        Update(session);
        return true;
    }

    public async ValueTask<bool> StartNewSession(SessionAddModel model)
    {

        int? lastSessionId = await GetLastSessionId(model.UserId);

        Session entity = new Session
        {
            StartDate = model.StartDate,
            EndDate = null,
            Refunds = model.Refunds,
            UserId = model.UserId,
            PreviousSessionId = lastSessionId,
            Last = true,
        };
        Add(entity);

        await updateGoodHabits(model.GoodHabitIds, [], entity);
        await updateBadHabits(model.BadHabitIds, [], entity);
        await updateTreats(model.TreatIdUnitPerSessionPairs, [], entity);

        //make previous not last
        if(lastSessionId is not null)
        {
            var prev = await Get(lastSessionId.Value);
            if (prev is null)
            {
                //well, that's awkward. Should exist! Something happened!
                return false;
            }
            Add(entity);
            prev.Last = false;
            Update(prev);
        }
        else
        {
            //just add, it's the first session overall
            Add(entity);
        }

        return true;
    }

    public async ValueTask<bool> FinishCurrentSession(string userId)
    {
        Session? entity = await GetAll()
            .Where(x => x.UserId == userId)
            .Where(x => x.Last)
            .FirstOrDefaultAsync();

        if (entity is null)
        {
            return false;
        }

        entity.EndDate = DateTime.Now;
        Update(entity);
        return true;
    }

    public async ValueTask<bool> IsOwnerOf(int id, string userId)
    {
        var model = await Get(id);
        return model is not null && model.UserId == userId;
    }

    public async ValueTask<bool> Exists(int id)
    {
        return (await Get(id)) is not null;
    }

    private Task<Session?> GetLatest(string userId)
    {
        return GetAll().Where(x => x.UserId == userId && x.Last).FirstOrDefaultAsync();
    }

    //stupid name...
    private IQueryable<Session> GetAllLatest(string userId)
    {
        return GetAll().Where(x => x.UserId == userId && x.Last);
    }

    private async Task<UpdateInfo> updateGoodHabits(int[] ids, int[] oldIds, Session session)
    {
        var updateInfo = new UpdateInfo { Success = true };

        int[] added = ids.Except(oldIds).ToArray();
        int[] deleted = oldIds.Except(ids).ToArray();
        
        foreach(var id in added)
        {
            session.SessionGoodHabits.Add(new SessionGoodHabit
            {
                GoodHabitId = id,
                Completed = false
            });
        }
        updateInfo.Added = added;

        //this didn't work
        //foreach(var id in deleted)
        //{
        //    session.SessionGoodHabits.Remove(new SessionGoodHabit { GoodHabitId = id, SessionId = session.Id });
        //}
        if (deleted.Length == 0) return updateInfo;

        var sghs = (await GetAllLatest(session.UserId)
            .Include(x => x.SessionGoodHabits
                .Where(x => deleted.Contains(x.GoodHabitId)))
            .FirstOrDefaultAsync())!.SessionGoodHabits;

        foreach(SessionGoodHabit sgh in sghs)
        {
            session.SessionGoodHabits.Remove(sgh);
        }

        updateInfo.Removed = deleted;
        return updateInfo;
    }

    private async Task<UpdateInfo> updateBadHabits(int[] ids, int[] oldIds, Session session)
    {
        var updateInfo = new UpdateInfo { Success = true };

        int[] added = ids.Except(oldIds).ToArray();
        int[] deleted = oldIds.Except(ids).ToArray();
        
        foreach(var id in added)
        {
            session.SessionBadHabits.Add(new SessionBadHabit
            {
                BadHabitId = id,
                Failed = false
            });
        }
        updateInfo.Added = added;

        //this didn't work
        //foreach(var id in deleted)
        //{
        //    session.SessionBadHabits.Remove(new SessionBadHabit { BadHabitId = id, SessionId = session.Id });
        //}
        if (deleted.Length == 0) return updateInfo;

        var sbhs = (await GetAllLatest(session.UserId)
            .Include(x => x.SessionBadHabits
                .Where(x => deleted.Contains(x.BadHabitId)))
            .FirstOrDefaultAsync())!.SessionBadHabits;

        foreach(SessionBadHabit sbh in sbhs)
        {
            session.SessionBadHabits.Remove(sbh);
        }

        updateInfo.Removed = deleted;
        return updateInfo;
    }

    private async Task<UpdateInfo> updateTreats(Tuple<int, byte>[] idsAndUnitsPerSession, int[] oldIds, Session session)
    {
        var updateInfo = new UpdateInfo { Success = true };

        var ids = idsAndUnitsPerSession.Select(x => x.Item1);

        Tuple<int, byte>[] added = idsAndUnitsPerSession.Where(x => !oldIds.Contains(x.Item1)).ToArray();
        int[] deleted = oldIds.Except(ids).ToArray();

        foreach(var IdAndUnitsPerSession in added)
        {
            session.SessionTreats.Add(new SessionTreat
            {
                TreatId = IdAndUnitsPerSession.Item1,
                UnitsLeft = IdAndUnitsPerSession.Item2
            });
        }
        updateInfo.Added = ids.ToArray();

        //this didn't work
        //foreach(var id in deleted)
        //{
        //    session.SessionTreats.Remove(new SessionTreat { TreatId = id, SessionId = session.Id });
        //}
        if (deleted.Length == 0) return updateInfo;

        var strs = (await GetAllLatest(session.UserId)
            .Include(x => x.SessionTreats
                .Where(x => deleted.Contains(x.TreatId)))
            .FirstOrDefaultAsync())!.SessionTreats;

        foreach(SessionTreat str in strs)
        {
            session.SessionTreats.Remove(str);
        }

        updateInfo.Removed = deleted;
        return updateInfo;
    }
}
