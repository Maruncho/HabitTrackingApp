
namespace HTApp.Core.Contracts;

public interface IBadHabitRepository<UserIdType, ModelIdType>
    : _ICommon<ModelIdType, BadHabitInputModel<UserIdType>>
{
    public Task<BadHabitModel[]> GetAll(UserIdType userId);
}
