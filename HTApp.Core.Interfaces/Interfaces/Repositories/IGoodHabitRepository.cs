
namespace HTApp.Core.Contracts;

public interface IGoodHabitRepository<UserIdType, ModelId>
    : _ICommon<ModelId, GoodHabitInputModel<UserIdType>>
{
    public Task<GoodHabitModel<ModelId>[]> GetAll(UserIdType userId);
}
