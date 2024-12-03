namespace HTApp.Core.API;

public interface IGoodHabitService
{
    public Task<Response<GoodHabitModel[]>> GetAll(string userId);
    public ValueTask<Response<GoodHabitInputModel>> GetInputModel(int id, string userId);
    public ValueTask<Response> Add(GoodHabitInputModel model, string userId);
    public ValueTask<Response> Update(int id, GoodHabitInputModel model, string userId);
    public ValueTask<Response> Delete(int id, string userId);
}
