
namespace HTApp.Core.API;

public interface IGoodHabitRepository
    : _ICommon<int, GoodHabitInputModel>
{
    public Task<GoodHabitModel[]> GetAll(string userId);
}
