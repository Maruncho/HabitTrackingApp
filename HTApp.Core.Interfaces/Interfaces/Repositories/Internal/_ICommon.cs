namespace HTApp.Core.Contracts;

//Sadly I cannot make it internal, because public ones inherit it.
public interface _ICommon<ModelIdType, InputModel>
{
    public ValueTask Add(InputModel model);

    public ValueTask Update(ModelIdType id, InputModel model);

    public ValueTask Delete(ModelIdType id);
}
