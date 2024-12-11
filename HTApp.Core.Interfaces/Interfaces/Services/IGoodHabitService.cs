namespace HTApp.Core.API;

public interface IGoodHabitService : IGoodHabitSubject
{
    public Task<Response<GoodHabitModel[]>> GetAll(string userId);
    public Task<Response<int[]>> GetAllIds(string userId, bool onlyActive = false);

    public Task<Response<GoodHabitInputModel>> GetInputModel(int id, string userId);
    public Task<Response<GoodHabitLogicModel>> GetLogicModel(int id, string userId);

    public Task<Response> Add(GoodHabitInputModel model, string userId);
    public Task<Response> Update(int id, GoodHabitInputModel model, string userId);
    public Task<Response> Delete(int id, string userId);

    public Task<bool> Exists(int id);
    public Task<bool> IsOwnerOf(int id, string userId);
}
