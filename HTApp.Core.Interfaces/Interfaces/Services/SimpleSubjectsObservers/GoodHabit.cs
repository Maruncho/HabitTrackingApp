namespace HTApp.Core.API;

//Subjects do SaveChanges. It's implicit, so be careful.
//This probably breaks the pattern, because if one thing fails, everything fails without the others knowing why.
public interface IGoodHabitSubject
{
    public void SubscribeToStatusChange(IGoodHabitObserver observer);
    public void UnsubscribeToStatusChange(IGoodHabitObserver observer);
    public Task<Response[]> NotifyStatusChange(bool isActive, string userId);
}

public interface IGoodHabitObserver
{
    public ValueTask<Response> NotifyWhenStatusChange(bool isActive, string userId);
}
