namespace HTApp.Core.API;

//Helps with error messages in the services. They should not be mandatory and the repositories must test for those either way.
public interface _PredicatesExtra<ModelId>
{
    public ValueTask<bool> Exists(ModelId id);
    public ValueTask<bool> IsOwnerOf(ModelId id, string userId);
}
