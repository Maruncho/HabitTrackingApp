using HTApp.Core.API;

namespace HTApp.Core.Tests.Services.Implementations;

internal class GoodGoodHabitRepository : IGoodHabitRepository
{
    public ValueTask<bool> Add(GoodHabitInputModel model)
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

    public Task<GoodHabitModel[]> GetAll(string userId)
    {
        return Task.FromResult(Array.Empty<GoodHabitModel>());
    }

    public Task<int[]> GetAllIds(string userid, bool onlyActive)
    {
        return Task.FromResult(Array.Empty<int>());
    }

    public ValueTask<GoodHabitInputModel?> GetInputModel(int id)
    {
        return ValueTask.FromResult<GoodHabitInputModel?>(null);
    }

    public Task<GoodHabitLogicModel?> GetLogicModel(int id)
    {
        return Task.FromResult<GoodHabitLogicModel?>(null);
    }

    public ValueTask<bool> IsOwnerOf(int id, string userId)
    {
        return ValueTask.FromResult(false);
    }

    public ValueTask<bool> Update(int id, GoodHabitInputModel model)
    {
        return ValueTask.FromResult(false);
    }
}
