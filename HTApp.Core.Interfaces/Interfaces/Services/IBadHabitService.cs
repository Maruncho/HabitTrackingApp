namespace HTApp.Core.API;

public interface IBadHabitService : IBadHabitSubject
{
    public Task<Response<BadHabitModel[]>> GetAll(string userId);
    public Task<Response<int[]>> GetAllIds(string userId);

    public Task<Response<BadHabitInputModel>> GetInputModel(int id, string userId);
    public Task<Response<BadHabitLogicModel>> GetLogicModel(int id, string userId);

    public Task<Response> Add(BadHabitInputModel model, string userId);
    public Task<Response> Update(int id, BadHabitInputModel model, string userId);
    public Task<Response> Delete(int id, string userId);

    public Task<bool> Exists(int id);
    public Task<bool> IsOwnerOf(int id, string userId);
}
