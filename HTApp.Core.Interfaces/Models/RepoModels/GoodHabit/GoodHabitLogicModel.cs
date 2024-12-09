namespace HTApp.Core.API;

public class GoodHabitLogicModel
{

    public required int Id { get; set; }

    public int CreditsSuccess { get; set; }

    public int CreditsFail { get; set; }

    public bool IsActive { get; set; }
}
