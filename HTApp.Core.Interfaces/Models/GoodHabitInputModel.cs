namespace HTApp.Core.Contracts;

public class GoodHabitInputModel<UserIdType> : GoodHabitModel
{
    public required UserIdType UserId { get; set; }
}
