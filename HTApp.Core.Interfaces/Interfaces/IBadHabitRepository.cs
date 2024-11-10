
namespace HTApp.Core.Contracts;

public interface IBadHabitRepository<UserIdType, ModelIdType, SourceEntity> : IGenericRepository<SourceEntity, ModelIdType>
{
    public Task<BadHabitSimple[]> GetSimpleAll(UserIdType userId);
}
