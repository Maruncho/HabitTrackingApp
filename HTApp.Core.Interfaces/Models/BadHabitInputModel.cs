namespace HTApp.Core.Contracts;

public class BadHabitInputModel<UserIdType> : BadHabitModel
{
    public required UserIdType UserId { get; set; }
}
