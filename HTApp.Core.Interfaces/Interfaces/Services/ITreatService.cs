namespace HTApp.Core.API;

public interface ITreatService : ITreatSubject
{
    public Task<Response<TreatModel[]>> GetAll(string userId);
    public Task<Response<Tuple<int, byte>[]>> GetAllIdAndUnitsLeftPairs(string userId);

    public Task<Response<TreatInputModel>> GetInputModel(int id, string userId);
    public Task<Response<TreatLogicModel>> GetLogicModel(int id, string userId);

    public Task<Response> Add(TreatInputModel model, string userId);
    public Task<Response> Update(int id, TreatInputModel model, string userId);
    public Task<Response> Delete(int id, string userId);

    public Task<bool> Exists(int id);
    public Task<bool> IsOwnerOf(int id, string userId);
}
