namespace HTApp.Core.API;

public class GoodHabitModel
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    public int CreditsSuccess { get; set; }

    public int CreditsFail { get; set; }

    public bool IsActive { get; set; } = true;
}
