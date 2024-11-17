namespace HTApp.Core.Contracts;

public class TransactionInputModel
{
    public int Id { get; set; }

    public required string Type { get; set; }

    public int Amount { get; set; }
}
