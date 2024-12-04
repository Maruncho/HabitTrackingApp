namespace HTApp.Core.API;

public interface IBadHabitService
{
    public ValueTask<Response<BadHabitModel[]>> GetAll(string userId);
    public ValueTask<Response<BadHabitInputModel>> GetInputModel(int id, string userId);
    public ValueTask<Response> Add(BadHabitInputModel model, string userId);
    public ValueTask<Response> Update(int id, BadHabitInputModel model, string userId);
    public ValueTask<Response> Delete(int id, string userId);
}
