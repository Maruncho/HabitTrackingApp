namespace HTApp.Core.API;

public interface IGoodHabitService
{
    public ValueTask<Response<GoodHabitModel[]>> GetAll(string userId);
    public ValueTask<Response<GoodHabitInputModel>> GetInputModel(int id, string userId);
    public ValueTask<Response> Add(GoodHabitInputModel model, string userId);
    public ValueTask<Response> Update(int id, GoodHabitInputModel model, string userId);
    public ValueTask<Response> Delete(int id, string userId);
}
