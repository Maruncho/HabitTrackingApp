namespace HTApp.Core.Contracts;

public interface ISessionRepository<UserIdType, ModelId, GoodHabitModelId, BadHabitModelId, TransactionModelId, TreatModelId>
{
    public Task<SessionModel<ModelId, GoodHabitModelId, BadHabitModelId, TransactionModelId, TreatModelId>?>
        GetCurrentSession(UserIdType userId);

    public Task<SessionUpdateModel<ModelId, UserIdType, GoodHabitModelId, BadHabitModelId, TransactionModelId, TreatModelId>?>
        GetCurrentSessionUpdateModel(UserIdType userId);

    public ValueTask<bool> Add(SessionAddModel<ModelId, UserIdType, GoodHabitModelId, BadHabitModelId, TransactionModelId, TreatModelId> model);

    public ValueTask<bool> Update(SessionUpdateModel<ModelId, UserIdType, GoodHabitModelId, BadHabitModelId, TransactionModelId, TreatModelId> model);

    public ValueTask<bool> DeleteCurrentAndMakePreviousCurrent(UserIdType userId);
}
