namespace HTApp.Core.Contracts;

public interface ICommon_AddableOnly<ModelIdType, InputModel>
{
    public ValueTask Add(InputModel model);
}
