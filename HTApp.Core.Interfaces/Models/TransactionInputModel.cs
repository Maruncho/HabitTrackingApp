namespace HTApp.Core.API;

public class TransactionInputModel
{
    public required string Type { get; set; }

    public int Amount { get; set; }

    public required string UserId { get; set; }
}
