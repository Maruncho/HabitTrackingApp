using HTApp.Infrastructure.EntityModels.Core;

namespace HTApp.Core.Contracts.Interfaces
{
    public interface IGoodHabitRepository<UserIdType, ModelIdType, SourceEntity> : IGenericRepository<SourceEntity, ModelIdType>
    {
        public Task<GoodHabitSimple[]> GetSimpleAll(UserIdType userId);
    }
}
