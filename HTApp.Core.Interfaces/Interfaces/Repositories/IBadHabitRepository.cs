
namespace HTApp.Core.Contracts;

public interface IBadHabitRepository<UserIdType, ModelIdType, SourceEntity>
    : _ICommon<ModelIdType, BadHabitInputModel<UserIdType>>
{
    public Task<BadHabitModel[]> GetAll(UserIdType userId);
}
