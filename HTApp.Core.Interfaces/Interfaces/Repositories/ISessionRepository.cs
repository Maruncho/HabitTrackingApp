namespace HTApp.Core.Contracts;

public interface ISessionRepository<UserIdType, ModelId, GoodHabitModelId, BadHabitModelId, TransactionModelId, TreatModelId>
    : _ICommon<ModelId, SessionInputModel<UserIdType, GoodHabitModelId, BadHabitModelId, TransactionModelId, TreatModelId>>
{
    public ValueTask<SessionModel<ModelId, GoodHabitModelId, BadHabitModelId, TransactionModelId, TreatModelId>>
        GetCurrentSession(UserIdType userId);
}
