
namespace HTApp.Core.Contracts;

public interface IBadHabitRepository<UserIdType, ModelId>
    : _ICommon<ModelId, BadHabitInputModel<UserIdType>>
{
    public Task<BadHabitModel<ModelId>[]> GetAll(UserIdType userId);
}
