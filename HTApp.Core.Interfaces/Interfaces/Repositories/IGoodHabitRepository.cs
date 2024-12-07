
namespace HTApp.Core.API;

public interface IGoodHabitRepository
    : _ICommon<int, GoodHabitInputModel>
{
    public Task<GoodHabitModel[]> GetAll(string userId);

    public Task<int[]> GetAllIds(string userid, bool onlyActive);

    public Task<GoodHabitLogicModel?> GetLogicModel(int id);
}
