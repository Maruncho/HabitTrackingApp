namespace HTApp.Core.API;

public class SessionAddModel
{
    public DateTime StartDate { get; set; }

    public byte Refunds { get; set; }

    public required int[] GoodHabitIds{ get; set; }

    public required int[] BadHabitIds { get; set; }

    public required Tuple<int, byte>[] TreatIdUnitPerSessionPairs { get; set; }

    public required string UserId { get; set; }

    public int? PreviousSessionId { get; set; }
}
