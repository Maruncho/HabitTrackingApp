
namespace HTApp.Core.Contracts;

public interface IGoodHabitRepository
    : _ICommon<int, GoodHabitInputModel>
{
    public Task<GoodHabitModel[]> GetAll(string userId);
}
