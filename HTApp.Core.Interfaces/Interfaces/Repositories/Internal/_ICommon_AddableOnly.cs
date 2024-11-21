namespace HTApp.Core.Contracts;

//Not meant to be used as an abstraction (polymorphism).
//Sadly I cannot make it internal, because public ones inherit it.
public interface ICommon_AddableOnly<ModelId, InputModel>
{
    public ValueTask<bool> Add(InputModel model);
}
