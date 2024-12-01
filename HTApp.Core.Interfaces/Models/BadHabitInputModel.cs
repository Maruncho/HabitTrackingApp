namespace HTApp.Core.Contracts;

public class BadHabitInputModel
{
    public required string Name { get; set; }

    public int CreditsSuccess { get; set; }

    public int CreditsFail { get; set; }

    public required string UserId { get; set; }
}
