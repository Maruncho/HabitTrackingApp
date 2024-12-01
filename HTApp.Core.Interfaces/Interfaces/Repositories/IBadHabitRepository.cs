
namespace HTApp.Core.Contracts;

public interface IBadHabitRepository
    : _ICommon<int, BadHabitInputModel>
{
    public Task<BadHabitModel[]> GetAll(string userId);
}
