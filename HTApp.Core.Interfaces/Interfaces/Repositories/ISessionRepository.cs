using HTApp.Core.API.Models.RepoModels;

namespace HTApp.Core.API;

public interface ISessionRepository : _PredicatesExtra<int>
{
    public Task<SessionModel?> GetLastSession(string userId, bool isNotFinished);
    public Task<int?> GetLastSessionId(string userId);
    public Task<SessionIsFinishedModel?> GetIsLastSessionFinished(string userId);

    public Task<byte?> GetJustRefunds(string userId);
    public Task<bool> DecrementRefunds(string userId);

    public Task<int[]?> GetGoodHabitIds(string userId);
    public Task<SessionHabitCreditsModel[]?> GetGoodHabitsFailed(string userId);
    public Task<bool?> GetGoodHabitCompleted(int id, string userId);
    public Task<UpdateInfo> UpdateGoodHabits(int[] ids, string userId);
    public Task<bool> UpdateGoodHabit(int id, bool success, string userId);

    public Task<int[]?> GetBadHabitIds(string userId);
    public Task<SessionHabitCreditsModel[]?> GetBadHabitsSuccess(string userId);
    public Task<bool?> GetBadHabitFailed(int id, string userId);
    public Task<UpdateInfo> UpdateBadHabits(int[] ids, string userId);
    public Task<bool> UpdateBadHabit(int id, bool fail, string userId);

    public Task<int[]?> GetTreatIds(string userId);
    public Task<byte?> GetTreatUnitsLeft(int id, string userId);
    public Task<UpdateInfo> UpdateTreats(Tuple<int, byte>[] idsAndUnitsPerSession, string userId);
    public Task<bool> DecrementTreat(int id, string userId);
    public Task<bool> IncrementTreat(int id, string userId);

    public ValueTask<bool> StartNewSession(SessionAddModel model);
    public ValueTask<bool> FinishCurrentSession(string userId);
}
