namespace HTApp.Core.API;

public interface ITreatService : ITreatSubject
{
    public ValueTask<Response<TreatModel[]>> GetAll(string userId);
    public ValueTask<Response<Tuple<int, byte>[]>> GetAllIdAndUnitsLeftPairs(string userId);

    public ValueTask<Response<TreatInputModel>> GetInputModel(int id, string userId);
    public ValueTask<Response<TreatLogicModel>> GetLogicModel(int id, string userId);

    public ValueTask<Response> Add(TreatInputModel model, string userId);
    public ValueTask<Response> Update(int id, TreatInputModel model, string userId);
    public ValueTask<Response> Delete(int id, string userId);

    public ValueTask<bool> Exists(int id);
    public ValueTask<bool> IsOwnerOf(int id, string userId);
}
