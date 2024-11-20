namespace HTApp.Core.Contracts;

public class SessionUpdateModel<ModelId, UserIdType, GdHId, BdHId, TrsId, TrtId>
    where GdHId : notnull
    where BdHId : notnull
    where TrtId : notnull
{
    public required ModelId Id { get; set; }

    public DateTime? EndDate { get; set; }

    public byte Refunds { get; set; }

    public required Dictionary<GdHId, bool> GoodHabitIdCompletedPairs { get; set; }

    public required Dictionary<BdHId, bool> BadHabitIdFailedPairs { get; set; }

    public required Dictionary<TrtId, byte> TreatIdUnitsLeftPairs { get; set; }

    public ModelId? PreviousSessionId { get; set; }

    public required UserIdType UserId { get; set; }
}
