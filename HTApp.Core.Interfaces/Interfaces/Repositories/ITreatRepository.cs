namespace HTApp.Core.API;

public interface ITreatRepository
    : _ICommon<int, TreatInputModel>
{
    public Task<TreatModel[]> GetAll(string userId);

    public Task<Tuple<int, byte>[]> GetAllIdAndQuantityPerSessionPairs(string userId);

    public Task<TreatLogicModel?> GetLogicModel(int id);
}
