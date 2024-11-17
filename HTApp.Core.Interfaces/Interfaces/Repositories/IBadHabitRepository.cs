
namespace HTApp.Core.Contracts;

public interface IBadHabitRepository<UserIdType, ModelIdType, SourceEntity>
    : _ICommon<ModelIdType, BadHabitModel>
{
    public Task<BadHabitModel> Get(UserIdType userId);
    public Task<BadHabitModel[]> GetAll(UserIdType userId);
}
