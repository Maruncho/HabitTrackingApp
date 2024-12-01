namespace HTApp.Core.Contracts;

public class SessionUpdateModel
{
    public required int Id { get; set; }

    public byte Refunds { get; set; }

    public required Dictionary<int, bool> GoodHabitIdCompletedPairs { get; set; }

    public required Dictionary<int, bool> BadHabitIdFailedPairs { get; set; }

    public required Dictionary<int, byte> TreatIdUnitsLeftPairs { get; set; }

    public required HashSet<int> TransactionIds { get; set; }

    public int? PreviousSessionId { get; set; }

    public required string UserId { get; set; }
}
