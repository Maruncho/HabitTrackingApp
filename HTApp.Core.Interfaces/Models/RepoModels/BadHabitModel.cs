namespace HTApp.Core.API;

public class BadHabitModel
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    public int CreditsSuccess { get; set; }

    public int CreditsFail { get; set; }
}
