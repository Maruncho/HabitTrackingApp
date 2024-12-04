namespace HTApp.Core.API;

public class TransactionServiceResponse
{
    public TransactionModel[] Models { get; set; } = null!;

    public int PageNumber { get; set; }

    public bool HasNext { get; set; }
}
