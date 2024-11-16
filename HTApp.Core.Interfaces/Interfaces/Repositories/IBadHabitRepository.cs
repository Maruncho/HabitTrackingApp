
namespace HTApp.Core.Contracts;

public interface IBadHabitRepository<UserIdType, ModelIdType, SourceEntity>
{
    public Task<BadHabitModel[]> GetAll(UserIdType userId);
}
