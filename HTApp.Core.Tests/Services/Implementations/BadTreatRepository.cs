﻿using HTApp.Core.API;

namespace HTApp.Core.Tests.Services.Implementations;

internal class BadTreatRepository : ITreatRepository
{
    public ValueTask<bool> Add(TreatInputModel model)
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

    public Task<TreatModel[]> GetAll(string userId)
    {
        return Task.FromResult(Array.Empty<TreatModel>());
    }

    public Task<Tuple<int, byte>[]> GetAllIdAndQuantityPerSessionPairs(string userId)
    {
        return Task.FromResult(Array.Empty<Tuple<int, byte>>());
    }

    public ValueTask<TreatInputModel?> GetInputModel(int id)
    {
        return ValueTask.FromResult<TreatInputModel?>(null);
    }

    public Task<TreatLogicModel?> GetLogicModel(int id)
    {
        return Task.FromResult<TreatLogicModel?>(null);
    }

    public ValueTask<bool> IsOwnerOf(int id, string userId)
    {
        return ValueTask.FromResult(false);
    }

    public ValueTask<bool> Update(int id, TreatInputModel model)
    {
        return ValueTask.FromResult(false);
    }
}
