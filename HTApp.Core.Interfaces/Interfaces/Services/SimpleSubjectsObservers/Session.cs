namespace HTApp.Core.API;

//Subjects do SaveChanges. It's implicit, so be careful.
//This probably breaks the pattern, because if one thing fails, everything fails without the others knowing why.
public interface ISessionSubject
{
    public void SubscribeToMakeTransaction(ISessionObserver observer);
    public void UnsubscribeToMakeTransaction(ISessionObserver observer);
    public Task<Response[]> NotifyMakeTransaction(MakeTransactionInfo info);
}

public interface ISessionObserver
{
    public Task<Response> NotifyWhenMakeTransaction(MakeTransactionInfo info);
}
