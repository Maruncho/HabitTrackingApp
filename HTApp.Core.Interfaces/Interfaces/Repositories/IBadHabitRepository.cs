
namespace HTApp.Core.API;

public interface IBadHabitRepository
    : _ICommon<int, BadHabitInputModel>
{
    public Task<BadHabitModel[]> GetAll(string userId);

    public Task<int[]> GetAllIds(string userId);

    public Task<BadHabitLogicModel?> GetLogicModel(int id);
}
