namespace HTApp.Core.Contracts;

public interface ICommon_AddableOnly<ModelIdType, InputModel>
{
    public ValueTask<bool> Add(InputModel model);
}
