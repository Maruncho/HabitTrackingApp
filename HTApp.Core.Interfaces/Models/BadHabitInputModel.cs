namespace HTApp.Core.Contracts;

public class BadHabitInputModel<UserIdType>
{
    public required string Name { get; set; }

    public int CreditsSuccess { get; set; }

    public int CreditsFail { get; set; }

    public required UserIdType UserId { get; set; }
}
