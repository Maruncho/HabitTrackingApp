using HTApp.Core.Contracts;
using HTApp.Infrastructure.EntityModels;
using HTApp.Infrastructure.EntityModels.SessionModels;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace HTApp.Infrastructure.Repositories;

public class SessionRepository
    : RepositoryBase<Session, int>,
      ISessionRepository<string, int, int, int, int, int>
{

    public SessionRepository(ApplicationDbContext db) : base(db)
    {
    }

    public Task<SessionModel<int, int, int, int, int>?> GetCurrentSession(string userId)
    {
        return GetAll()
            .Where(x => x.UserId == userId)
            .Where(x => x.EndDate == null) //maybe a bit obscure, but that's how you know it's the current
            .Select(x => new SessionModel<int, int, int, int, int>
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

    public Task<SessionUpdateModel<int, string, int, int, int, int>?> GetCurrentSessionUpdateModel(string userId)
    {
        return GetAll()
            .Where(x => x.UserId == userId)
            .Where(x => x.EndDate == null) //maybe a bit obscure, but that's how you know it's the current
            .Select(x => new SessionUpdateModel<int, string, int, int, int, int>
            {
                Id = x.Id,
                UserId = x.UserId,
                EndDate = x.EndDate,
                Refunds = x.Refunds,
                PreviousSessionId = x.PreviousSessionId.GetValueOrDefault(), //templates cannot resolve this...
                GoodHabitIdCompletedPairs = x.SessionGoodHabits
                    .ToDictionary(gh => gh.GoodHabitId, gh => gh.Completed),
                BadHabitIdFailedPairs = x.SessionBadHabits
                    .ToDictionary(bh => bh.BadHabitId, bh => bh.Failed),
                TreatIdUnitsLeftPairs = x.SessionTreats
                    .ToDictionary(tr => tr.TreatId, tr => tr.UnitsLeft)
            })
            .FirstOrDefaultAsync();
    }

    public ValueTask<bool> Add(SessionAddModel<int, string, int, int, int, int> model)
    {
        Session entity = new Session
        {
            StartDate = model.StartDate,
            EndDate = null,
            Refunds = model.Refunds,
            UserId = model.UserId,
            PreviousSessionId = model.PreviousSessionId,
        };
        foreach(var id in model.GoodHabitIds)
        {
            entity.SessionGoodHabits.Add(new SessionGoodHabit
            {
                GoodHabitId = id,
                Completed = false
            });
        }
        foreach(var id in model.BadHabitIds)
        {
            entity.SessionGoodHabits.Add(new SessionGoodHabit
            {
                GoodHabitId = id,
                Completed = false
            });
        }
        foreach(var IdAndUnitsPerSession in model.TreatIdUnitPerSessionPairs)
        {
            entity.SessionTreats.Add(new SessionTreat
            {
                TreatId = IdAndUnitsPerSession.Item1,
                UnitsLeft = IdAndUnitsPerSession.Item2
            });
        }

        Add(entity);
        return ValueTask.FromResult(true);
    }

    public async ValueTask<bool> Update(SessionUpdateModel<int, string, int, int, int, int> model)
    {
        Session? entity = await GetAll()
            .Where(x => x.UserId == model.UserId)
            .Include(x => x.SessionGoodHabits)
            .Include(x => x.SessionBadHabits)
            .Include(x => x.SessionTreats)
            .FirstOrDefaultAsync();

        if (entity == null)
        {
            return false;
        }

        entity.EndDate = model.EndDate;
        entity.Refunds = model.Refunds;
        entity.PreviousSessionId = model.PreviousSessionId;
        foreach (var gh in entity.SessionGoodHabits)
        {
            gh.Completed = model.GoodHabitIdCompletedPairs[gh.GoodHabitId];
        }
        foreach (var bh in entity.SessionBadHabits)
        {
            bh.Failed = model.BadHabitIdFailedPairs[bh.BadHabitId];
        }
        foreach (var tr in entity.SessionTreats)
        {
            tr.UnitsLeft = model.TreatIdUnitsLeftPairs[tr.TreatId];
        }

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
            //do nothing, it's for the best
            return false;
        }
    }
}
