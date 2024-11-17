namespace HTApp.Core.Contracts;

//Sadly I cannot make it internal, because public ones inherit it.
public interface _ICommon<ModelIdType, InputModel> : ICommon_AddableOnly<ModelIdType, InputModel>
{
    public ValueTask<InputModel?> GetInputModel(ModelIdType id);

    public ValueTask<bool> Update(ModelIdType id, InputModel model);

    public ValueTask<bool> Delete(ModelIdType id);
}
