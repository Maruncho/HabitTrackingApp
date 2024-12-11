namespace HTApp.Core.API;

//Helps with error messages in the services. They should not be mandatory and the repositories must test for those either way.
public interface _PredicatesExtra<ModelId>
{
    public Task<bool> Exists(ModelId id);
    public Task<bool> IsOwnerOf(ModelId id, string userId);
}
