namespace HTApp.Core.Contracts;

//Not meant to be used as an abstraction (polymorphism).
//Sadly I cannot make it internal, because public ones inherit it.
public interface _ICommon<ModelId, InputModel> : ICommon_AddableOnly<ModelId, InputModel>
{
    public ValueTask<InputModel?> GetInputModel(ModelId id);

    //Update is more like a PUT, than a PATCH. 
    public ValueTask<bool> Update(ModelId id, InputModel model);

    public ValueTask<bool> Delete(ModelId id);
}
