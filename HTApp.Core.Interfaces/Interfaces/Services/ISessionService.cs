namespace HTApp.Core.API;

public interface ISessionService : ISessionSubject,
    IGoodHabitObserver, IBadHabitObserver, ITreatObserver
{
    public ValueTask<Response<SessionModel>> GetLastSession(string userId, bool mustNotBeFinished);
    public ValueTask<ResponseStruct<int>> GetLastSessionId(string userId, bool isNotFinished);

    /* Implemented through the observers

    private UpdateGoodHabits();

    private UpdateBadHabits();

    private UpdateTreats();
    */

    public ValueTask<Response> UpdateGoodHabit(int id, bool success, string userId);
    public ValueTask<Response> UpdateBadHabit(int id, bool fail, string userId);
    public ValueTask<Response> BuyTreat(int id, string userId);
    public ValueTask<Response> RefundTreat(int id, string userId);

    public ValueTask<Response> StartNewSession(string userId);
    public ValueTask<Response> FinishCurrentSession(string userId);
}
