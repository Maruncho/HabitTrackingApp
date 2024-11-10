
namespace HTApp.Core.Contracts;

public interface IGoodHabitRepository<UserIdType, ModelIdType, SourceEntity> : IGenericRepository<SourceEntity, ModelIdType>
{
    public Task<GoodHabitSimple[]> GetSimpleAll(UserIdType userId);
}
