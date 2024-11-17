namespace HTApp.Core.Contracts;

public interface ITreatRepository<UserIdType, ModelIdType, SourceEntity>
    : _ICommon<ModelIdType, TreatModel>
{
    public Task<TreatModel[]> GetAll(UserIdType userId);
}
