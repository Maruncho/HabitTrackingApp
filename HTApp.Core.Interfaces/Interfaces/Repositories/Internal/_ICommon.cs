namespace HTApp.Core.API;

//Not meant to be used as an abstraction (polymorphism).
//Sadly I cannot make it internal, because public ones inherit it.
public interface _ICommon<ModelId, InputModel> : ICommon_AddableOnly<InputModel, ModelId>
{
    public Task<InputModel?> GetInputModel(ModelId id);

    //Update is more like a PUT, than a PATCH. 
    public Task<bool> Update(ModelId id, InputModel model);

    public Task<bool> Delete(ModelId id);
}
