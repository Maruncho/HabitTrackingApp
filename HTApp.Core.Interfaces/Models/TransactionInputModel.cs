namespace HTApp.Core.Contracts;

public class TransactionInputModel
{
    public required string Type { get; set; }

    public int Amount { get; set; }

    public required string UserId { get; set; }
}
