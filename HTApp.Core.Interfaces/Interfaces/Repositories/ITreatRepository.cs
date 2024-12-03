namespace HTApp.Core.API;

public interface ITreatRepository
    : _ICommon<int, TreatInputModel>
{
    public Task<TreatModel[]> GetAll(string userId);
}
