namespace HTApp.Core.API;

public class TransactionOptions
{
    public int AdditionalEntries { get; set; } = 0;
    public string FilterTypeName { get; set; } = "";
    public int? FromSessionId { get; set; } = null;
}
