namespace HTApp.Core.Contracts;

public interface ITreatRepository<UserIdType, ModelId>
    : _ICommon<ModelId, TreatInputModel<UserIdType>>
{
    public Task<TreatModel<ModelId>[]> GetAll(UserIdType userId);
}
