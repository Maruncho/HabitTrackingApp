namespace HTApp.Core.Contracts;

public class SessionAddModel<ModelIdNullable, UserIdType, GdHId, BdHId, TrsId, TrtId>
{
    public DateTime StartDate { get; set; }

    public byte Refunds { get; set; }

    public required GdHId[] GoodHabitIds{ get; set; }

    public required BdHId[] BadHabitIds { get; set; }

    public required Tuple<TrtId, byte>[] TreatIdUnitPerSessionPairs { get; set; }

    public required UserIdType UserId { get; set; }

    public ModelIdNullable? PreviousSessionId { get; set; }
}
