namespace HTApp.Core.Contracts;

public interface ITreatRepository<UserIdType, ModelIdType>
    : _ICommon<ModelIdType, TreatInputModel<UserIdType>>
{
    public Task<TreatModel[]> GetAll(UserIdType userId);
}
