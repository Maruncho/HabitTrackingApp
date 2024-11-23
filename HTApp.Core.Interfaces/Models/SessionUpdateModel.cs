using System.Data.SqlTypes;

namespace HTApp.Core.Contracts;

public class SessionUpdateModel<ModelId, ModelIdNullable, UserIdType, GdHId, BdHId, TrsId, TrtId>
    where GdHId : notnull
    where BdHId : notnull
    where TrtId : notnull
{
    public required ModelId Id { get; set; }

    public byte Refunds { get; set; }

    public required Dictionary<GdHId, bool> GoodHabitIdCompletedPairs { get; set; }

    public required Dictionary<BdHId, bool> BadHabitIdFailedPairs { get; set; }

    public required Dictionary<TrtId, byte> TreatIdUnitsLeftPairs { get; set; }

    public required HashSet<TrsId> TransactionIds { get; set; }

    public ModelIdNullable? PreviousSessionId { get; set; }

    public required UserIdType UserId { get; set; }
}
