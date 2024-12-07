namespace HTApp.Core.API;

public interface IBadHabitService : IBadHabitSubject
{
    public ValueTask<Response<BadHabitModel[]>> GetAll(string userId);
    public ValueTask<Response<int[]>> GetAllIds(string userId);

    public ValueTask<Response<BadHabitInputModel>> GetInputModel(int id, string userId);
    public ValueTask<Response<BadHabitLogicModel>> GetLogicModel(int id, string userId);

    public ValueTask<Response> Add(BadHabitInputModel model, string userId);
    public ValueTask<Response> Update(int id, BadHabitInputModel model, string userId);
    public ValueTask<Response> Delete(int id, string userId);

    public ValueTask<bool> Exists(int id);
    public ValueTask<bool> IsOwnerOf(int id, string userId);
}
