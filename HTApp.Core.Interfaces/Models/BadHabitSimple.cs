namespace HTApp.Core.Contracts;

public class BadHabitSimple
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public int CreditsSuccess { get; set; }

    public int CreditsFail { get; set; }
}
