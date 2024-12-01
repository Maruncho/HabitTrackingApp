namespace HTApp.Core.Contracts;

public interface ITreatRepository
    : _ICommon<int, TreatInputModel>
{
    public Task<TreatModel[]> GetAll(string userId);
}
