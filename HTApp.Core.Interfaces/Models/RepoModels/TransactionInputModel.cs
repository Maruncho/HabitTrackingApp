namespace HTApp.Core.API;

public class TransactionInputModel
{
    public string Type { get; set; } = null!;

    public int Amount { get; set; }

    public string UserId { get; set; } = null!;
}
