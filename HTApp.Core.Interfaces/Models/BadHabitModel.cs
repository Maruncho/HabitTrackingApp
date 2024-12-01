namespace HTApp.Core.Contracts;

public class BadHabitModel
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    public int CreditsSuccess { get; set; }

    public int CreditsFail { get; set; }
}
