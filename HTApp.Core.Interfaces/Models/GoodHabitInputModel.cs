namespace HTApp.Core.Contracts;

public class GoodHabitInputModel<UserIdType>
{
    public required string Name { get; set; }

    public int CreditsSuccess { get; set; }

    public int CreditsFail { get; set; }

    public bool IsActive { get; set; } = true;

    public required UserIdType UserId { get; set; }
}
