using HTApp.Core.API;

namespace HTApp.Core.Tests.Services.Implementations;

public class BadBadHabitRepository : IBadHabitRepository
{
    public ValueTask<bool> Add(BadHabitInputModel model)
    {
        return ValueTask.FromResult(false);
    }

    public ValueTask<bool> Delete(int id)
    {
        return ValueTask.FromResult(false);
    }

    public ValueTask<bool> Exists(int id)
    {
        return ValueTask.FromResult(false);
    }

    public Task<BadHabitModel[]> GetAll(string userId)
    {
        return Task.FromResult(Array.Empty<BadHabitModel>());
    }

    public Task<int[]> GetAllIds(string userId)
    {
        return Task.FromResult(Array.Empty<int>());
    }

    public ValueTask<BadHabitInputModel?> GetInputModel(int id)
    {
        return ValueTask.FromResult<BadHabitInputModel?>(null);
    }

    public Task<BadHabitLogicModel?> GetLogicModel(int id)
    {
        return Task.FromResult<BadHabitLogicModel?>(null);
    }

    public ValueTask<bool> IsOwnerOf(int id, string userId)
    {
        return ValueTask.FromResult(false);
    }

    public ValueTask<bool> Update(int id, BadHabitInputModel model)
    {
        return ValueTask.FromResult(false);
    }
}
