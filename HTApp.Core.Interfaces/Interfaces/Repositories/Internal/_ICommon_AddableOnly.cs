namespace HTApp.Core.Contracts;

public interface ICommon_AddableOnly<ModelId, InputModel>
{
    public ValueTask<bool> Add(InputModel model);
}
