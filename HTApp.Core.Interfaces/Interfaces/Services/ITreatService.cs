namespace HTApp.Core.API;

public interface ITreatService
{
    public ValueTask<Response<TreatModel[]>> GetAll(string userId);
    public ValueTask<Response<TreatInputModel>> GetInputModel(int id, string userId);
    public ValueTask<Response> Add(TreatInputModel model, string userId);
    public ValueTask<Response> Update(int id, TreatInputModel model, string userId);
    public ValueTask<Response> Delete(int id, string userId);
}
