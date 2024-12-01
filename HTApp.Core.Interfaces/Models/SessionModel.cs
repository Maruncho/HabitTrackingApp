namespace HTApp.Core.Contracts;

public class SessionModel
{
    public required int Id { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public byte Refunds { get; set; }

    public required Tuple<int, bool>[] GoodHabitIdCompletedPairs { get; set; }

    public required Tuple<int, bool>[] BadHabitIdFailedPairs { get; set; }

    public required int[] TransactionIds { get; set; }

    public required Tuple<int, byte>[] TreatIdUnitsLeftPairs { get; set; }
}
