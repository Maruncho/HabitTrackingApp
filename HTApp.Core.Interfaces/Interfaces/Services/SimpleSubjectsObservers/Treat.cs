namespace HTApp.Core.API;

//Subjects do SaveChanges. It's implicit, so be careful.
//This probably breaks the pattern, because if one thing fails, everything fails without the others knowing why.
public interface ITreatSubject
{
    public void SubscribeToStatusChange(ITreatObserver observer);
    public void UnsubscribeToStatusChange(ITreatObserver observer);
    public Task<Response[]> NotifyStatusChange(string userId);
}

public interface ITreatObserver
{
    public ValueTask<Response> NotifyWhenStatusChange(string userId);
}
