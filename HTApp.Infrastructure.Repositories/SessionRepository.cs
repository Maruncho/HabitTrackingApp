using HTApp.Core.API;
using HTApp.Infrastructure.EntityModels;
using HTApp.Infrastructure.EntityModels.SessionModels;
using Microsoft.EntityFrameworkCore;

namespace HTApp.Infrastructure.Repositories;

public class SessionRepository
    : RepositoryBase<Session, int>,
      ISessionRepository
{

    public SessionRepository(ApplicationDbContext db) : base(db)
    {
    }

    public Task<SessionModel?> GetCurrentSession(string userId)
    {
        return GetAll()
            .Where(x => x.UserId == userId)
            .Where(x => x.EndDate == null) //maybe a bit obscure, but that's how you know it's the current
            .Select(x => new SessionModel
            {
                Id = x.Id,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Refunds = x.Refunds,
                GoodHabitIdCompletedPairs = x.SessionGoodHabits
                    .Select(gh => new Tuple<int, bool>(gh.GoodHabitId, gh.Completed))
                    .ToArray(),
                BadHabitIdFailedPairs = x.SessionBadHabits
                    .Select(bh => new Tuple<int, bool>(bh.BadHabitId, bh.Failed))
                    .ToArray(),
                TransactionIds = x.SessionTransactions
                    .Select(ts => ts.TransactionId)
                    .ToArray(),
                TreatIdUnitsLeftPairs = x.SessionTreats
                    .Select(tr => new Tuple<int, byte>(tr.TreatId, tr.UnitsLeft))
                    .ToArray()
            })
            .FirstOrDefaultAsync();
    }

    public Task<int?> GetCurrentSessionId(string userId)
    {
        return GetAll().Where(x => x.UserId == userId && x.EndDate == null).Select(x => (int?)x.Id ).FirstOrDefaultAsync();
    }

    public async Task<SessionUpdateModel?> GetCurrentSessionUpdateModel(string userId)
    {
        var x = await GetAll()
            .Where(x => x.UserId == userId)
            .Where(x => x.EndDate == null) //maybe a bit obscure, but that's how you know it's the current
            //This select is because EF Core is not flexible enough.
            .Select(x => new
            {
                x.Id, x.UserId, x.EndDate, x.Refunds, x.PreviousSessionId,
                SessionGoodHabits = x.SessionGoodHabits
                    .Select(x => new { x.GoodHabitId, x.Completed }),
                SessionBadHabits = x.SessionBadHabits
                    .Select(x => new { x.BadHabitId, x.Failed }),
                SessionTreats = x.SessionTreats
                    .Select(x => new { x.TreatId, x.UnitsLeft }),
                SessionTransactions = x.SessionTransactions
                    .Select(x => x.TransactionId)
            })
            .FirstOrDefaultAsync();

        if (x is null) return null;

        var model = new SessionUpdateModel
        {
            Id = x.Id,
            UserId = x.UserId,
            Refunds = x.Refunds,
            PreviousSessionId = x.PreviousSessionId,
            GoodHabitIdCompletedPairs = x.SessionGoodHabits
                .ToDictionary(gh => gh.GoodHabitId, gh => gh.Completed),
            BadHabitIdFailedPairs = x.SessionBadHabits
                .ToDictionary(bh => bh.BadHabitId, bh => bh.Failed),
            TreatIdUnitsLeftPairs = x.SessionTreats
                .ToDictionary(tr => tr.TreatId, tr => tr.UnitsLeft),
            TransactionIds = x.SessionTransactions.ToHashSet(),
        };

        return model;
    }

    public async ValueTask<bool> AddAndMakeCurrent(SessionAddModel model)
    {
        Session entity = new Session
        {
            StartDate = model.StartDate,
            EndDate = null,
            Refunds = model.Refunds,
            UserId = model.UserId,
            PreviousSessionId = model.PreviousSessionId,
        };
        Add(entity);

        foreach(var id in model.GoodHabitIds)
        {
            entity.SessionGoodHabits.Add(new SessionGoodHabit
            {
                SessionId = entity.Id,
                GoodHabitId = id,
                Completed = false
            });
        }
        foreach(var id in model.BadHabitIds)
        {
            entity.SessionBadHabits.Add(new SessionBadHabit
            {
                BadHabitId = id,
                Failed = false
            });
        }
        foreach(var IdAndUnitsPerSession in model.TreatIdUnitPerSessionPairs)
        {
            entity.SessionTreats.Add(new SessionTreat
            {
                SessionId = entity.Id,
                TreatId = IdAndUnitsPerSession.Item1,
                UnitsLeft = IdAndUnitsPerSession.Item2
            });
        }

        //finish previous
        if(model.PreviousSessionId is not null)
        {
            var prev = await Get(model.PreviousSessionId.Value);
            if (prev is null)
            {
                return false;
            }
            Add(entity);
            prev.EndDate = model.StartDate;
            Update(prev);
        }
        else
        {
            //just add, it's the first session overall
            Add(entity);
        }

        return true;
    }

    //Big boy. There is no functionality to share, so splitting is not necessary.
    public async ValueTask<bool> Update(SessionUpdateModel model)
    {
        Session? entity = await GetAll()
            .Where(x => x.Id == model.Id)
            .Include(x => x.SessionGoodHabits)
            .Include(x => x.SessionBadHabits)
            .Include(x => x.SessionTreats)
            .Include(x => x.SessionTransactions)
            .FirstOrDefaultAsync();

        if (entity == null)
        {
            return false;
        }

        entity.Refunds = model.Refunds;
        entity.PreviousSessionId = model.PreviousSessionId;
        foreach (var gh in entity.SessionGoodHabits)
        {
            var exists = model.GoodHabitIdCompletedPairs.TryGetValue(gh.GoodHabitId, out var value);
            if (!exists) Delete(gh);
            gh.Completed = value;
        }
        //Add new ones
        entity.SessionGoodHabits = entity.SessionGoodHabits.Concat(
            model.GoodHabitIdCompletedPairs
                .Select(x => x.Key)
                .Except(entity.SessionGoodHabits.Select(x => x.GoodHabitId))
                .Select(key => new SessionGoodHabit
                {
                    GoodHabitId = key,
                    Completed = model.GoodHabitIdCompletedPairs[key]
                })
        ).ToHashSet();

        foreach (var bh in entity.SessionBadHabits)
        {
            var exists = model.BadHabitIdFailedPairs.TryGetValue(bh.BadHabitId, out var value);
            if (!exists) Delete(bh);
            bh.Failed = value;
        }
        //Add new ones
        entity.SessionBadHabits = entity.SessionBadHabits.Concat(
            model.BadHabitIdFailedPairs
                .Select(x => x.Key)
                .Except(entity.SessionBadHabits.Select(x => x.BadHabitId))
                .Select(key => new SessionBadHabit
                {
                    BadHabitId = key,
                    Failed = model.BadHabitIdFailedPairs[key]
                })
        ).ToHashSet();

        foreach (var tr in entity.SessionTreats)
        {
            var exists = model.TreatIdUnitsLeftPairs.TryGetValue(tr.TreatId, out var value);
            if (!exists) Delete(tr);
            tr.UnitsLeft = value;
        }
        //Add new ones
        entity.SessionTreats = entity.SessionTreats.Concat(
            model.TreatIdUnitsLeftPairs
                .Select(x => x.Key)
                .Except(entity.SessionTreats.Select(x => x.TreatId))
                .Select(key => new SessionTreat
                {
                    TreatId = key,
                    UnitsLeft = model.TreatIdUnitsLeftPairs[key]
                })
        ).ToHashSet();

        foreach (var tr in entity.SessionTransactions)
        {
            var exists = model.TransactionIds.TryGetValue(tr.TransactionId, out var value);
            if (!exists) Delete(tr);
        }
        //Add new ones
        entity.SessionTransactions = entity.SessionTransactions.Concat(
            model.TransactionIds
                .Except(entity.SessionTransactions.Select(x => x.TransactionId))
                .Select(key => new SessionTransaction
                {
                    TransactionId = key,
                })
        ).ToHashSet();

        Update(entity);
        return true;
    }

    public async ValueTask<bool> DeleteCurrentAndMakePreviousCurrent(string userId)
    {
        Session? entity = await GetAll()
            .Where(x => x.UserId == userId)
            .Where(x => x.EndDate == null) //maybe a bit obscure, but that's how you know it's the current
            .Include(x => x.PreviousSession)
            .FirstOrDefaultAsync();

        if (entity is null)
        {
            return false;
        }

        if(entity.PreviousSession is not null)
        {
            //Make the previous the current
            entity.PreviousSession.EndDate = null;
            Update(entity.PreviousSession);

            Delete(entity);
            return true;
        }
        else
        {
            //do nothing, Business Logic requires one active Session.
            return false;
        }
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
}
