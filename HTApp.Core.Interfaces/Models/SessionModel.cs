namespace HTApp.Core.Contracts;

public class SessionModel<ModelId, GdHId, BdHId, TrsId, TrtId>
{
    public required ModelId Id { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public byte Refunds { get; set; }

    public required Tuple<GdHId, bool>[] GoodHabitIdCompletedPairs { get; set; }

    public required Tuple<BdHId, bool>[] BadHabitIdFailedPairs { get; set; }

    public required TrsId[] TransactionIds { get; set; }

    public required Tuple<TrtId, byte>[] TreatIdUnitsLeftPairs { get; set; }
}
