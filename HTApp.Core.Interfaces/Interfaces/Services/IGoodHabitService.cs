namespace HTApp.Core.API;

public interface IGoodHabitService : IGoodHabitSubject
{
    public ValueTask<Response<GoodHabitModel[]>> GetAll(string userId);
    public ValueTask<Response<int[]>> GetAllIds(string userId, bool onlyActive = false);

    public ValueTask<Response<GoodHabitInputModel>> GetInputModel(int id, string userId);
    public ValueTask<Response<GoodHabitLogicModel>> GetLogicModel(int id, string userId);

    public ValueTask<Response> Add(GoodHabitInputModel model, string userId);
    public ValueTask<Response> Update(int id, GoodHabitInputModel model, string userId);
    public ValueTask<Response> Delete(int id, string userId);

    public ValueTask<bool> Exists(int id);
    public ValueTask<bool> IsOwnerOf(int id, string userId);
}
