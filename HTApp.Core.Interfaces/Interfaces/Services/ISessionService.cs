namespace HTApp.Core.API;

public interface ISessionService : ISessionSubject,
    IGoodHabitObserver, IBadHabitObserver, ITreatObserver
{
    public Task<Response<SessionModel>> GetLastSession(string userId, bool mustNotBeFinished);
    public Task<ResponseStruct<int>> GetLastSessionId(string userId, bool isNotFinished);

    /* Implemented through the observers

    private UpdateGoodHabits();

    private UpdateBadHabits();

    private UpdateTreats();
    */

    public Task<Response> UpdateGoodHabit(int id, bool success, string userId);
    public Task<Response> UpdateBadHabit(int id, bool fail, string userId);
    public Task<Response> BuyTreat(int id, string userId);
    public Task<Response> RefundTreat(int id, string userId);

    public Task<Response> StartNewSession(string userId);
    public Task<Response> FinishCurrentSession(string userId);

    public Task<Response> RefreshIfDataIsNotInSync(string userId);
}
