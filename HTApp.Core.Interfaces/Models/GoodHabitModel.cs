namespace HTApp.Core.Contracts;

public class GoodHabitModel<ModelId>
{
    public required ModelId Id { get; set; }

    public required string Name { get; set; }

    public int CreditsSuccess { get; set; }

    public int CreditsFail { get; set; }

    public bool IsActive { get; set; } = true;
}
