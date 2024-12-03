namespace HTApp.Core.API;

//Not meant to be used as an abstraction (polymorphism).
//Sadly I cannot make it internal, because public ones inherit it.
public interface ICommon_AddableOnly<InputModel, ModelId> : _PredicatesExtra<ModelId>
{
    public ValueTask<bool> Add(InputModel model);
}
