namespace HTApp.Core.Contracts;

public class TransactionInputModel<UserIdType>
{
    public int Id { get; set; }

    public required string Type { get; set; }

    public int Amount { get; set; }

    public required UserIdType UserId { get; set; }
}
