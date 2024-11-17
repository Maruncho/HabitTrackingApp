
namespace HTApp.Core.Contracts;

public interface IGoodHabitRepository<UserIdType, ModelIdType, SourceEntity>
    : _ICommon<ModelIdType, GoodHabitInputModel<UserIdType>>
{
    public Task<GoodHabitModel[]> GetAll(UserIdType userId);
}
