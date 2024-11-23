namespace HTApp.Core.Contracts;

public interface ISessionRepository<UserIdType, ModelId, ModelIdNullable, GoodHabitModelId, BadHabitModelId, TransactionModelId, TreatModelId>
    where GoodHabitModelId : notnull
    where BadHabitModelId : notnull
    where TreatModelId : notnull
{
    public Task<SessionModel<ModelId, GoodHabitModelId, BadHabitModelId, TransactionModelId, TreatModelId>?>
        GetCurrentSession(UserIdType userId);

    public Task<ModelIdNullable> GetCurrentSessionId(UserIdType userId);

    public Task<SessionUpdateModel<ModelId, ModelIdNullable, UserIdType, GoodHabitModelId, BadHabitModelId, TransactionModelId, TreatModelId>?>
        GetCurrentSessionUpdateModel(UserIdType userId);

    public ValueTask<bool> AddAndMakeCurrent(SessionAddModel<ModelIdNullable, UserIdType, GoodHabitModelId, BadHabitModelId, TransactionModelId, TreatModelId> model);

    public ValueTask<bool> Update(SessionUpdateModel<ModelId, ModelIdNullable, UserIdType, GoodHabitModelId, BadHabitModelId, TransactionModelId, TreatModelId> model);

    public ValueTask<bool> DeleteCurrentAndMakePreviousCurrent(UserIdType userId);
}
