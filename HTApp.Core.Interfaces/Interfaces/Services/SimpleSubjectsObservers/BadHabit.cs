namespace HTApp.Core.API;

//Subjects do SaveChanges. It's implicit, so be careful.
//This probably breaks the pattern, because if one thing fails, everything fails without the others knowing why.
public interface IBadHabitSubject
{
    public void SubscribeToStatusChange(IBadHabitObserver observer);
    public void UnsubscribeToStatusChange(IBadHabitObserver observer);
    public Task<Response[]> NotifyStatusChange(string userId);
}

public interface IBadHabitObserver
{
    public ValueTask<Response> NotifyWhenStatusChange(string userId);
}
