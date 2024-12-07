using static HTApp.Core.API.ApplicationInvariants;

namespace HTApp.Core.API;
public class MakeTransactionInfo
{
    public int Amount { get; set; }

    public TransactionTypesEnum TransactionType { get; set; }

    public int SessionId { get; set; }

    public string UserId { get; set; } = null!;
}
