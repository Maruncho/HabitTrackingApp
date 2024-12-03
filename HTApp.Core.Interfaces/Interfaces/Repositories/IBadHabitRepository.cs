
namespace HTApp.Core.API;

public interface IBadHabitRepository
    : _ICommon<int, BadHabitInputModel>
{
    public Task<BadHabitModel[]> GetAll(string userId);
}
