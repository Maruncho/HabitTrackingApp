namespace HTApp.Core.API;

public class TransactionModel
{
    public required int Id { get; set; }

    public required string Type { get; set; }

    public required string Message { get; set; }

    public int Amount { get; set; }
}
