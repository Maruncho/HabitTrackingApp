using HTApp.Core.API;

namespace HTApp.Core.Tests.Services.Implementations;

public class GoodBadHabitRepository : IBadHabitRepository
{
    public ValueTask<bool> Add(BadHabitInputModel model)
    {
        return ValueTask.FromResult(true);
    }

    public ValueTask<bool> Delete(int id)
    {
        return ValueTask.FromResult(true);
    }

    public ValueTask<bool> Exists(int id)
    {
        return ValueTask.FromResult(true);
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
        return ValueTask.FromResult<BadHabitInputModel?>(new BadHabitInputModel());
    }

    public Task<BadHabitLogicModel?> GetLogicModel(int id)
    {
        return Task.FromResult<BadHabitLogicModel?>(new BadHabitLogicModel() { Id = 1});
    }

    public ValueTask<bool> IsOwnerOf(int id, string userId)
    {
        return ValueTask.FromResult(true);
    }

    public ValueTask<bool> Update(int id, BadHabitInputModel model)
    {
        return ValueTask.FromResult(true);
    }
}
