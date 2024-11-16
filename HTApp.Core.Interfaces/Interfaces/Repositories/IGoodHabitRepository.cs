
namespace HTApp.Core.Contracts;

public interface IGoodHabitRepository<UserIdType, ModelIdType, SourceEntity>
{
    public Task<GoodHabitModel[]> GetAll(UserIdType userId);
}
