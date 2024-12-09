namespace HTApp.Core.API;

public class SessionAddModel
{
    public DateTime StartDate { get; set; }

    public byte Refunds { get; set; }

    public int[] GoodHabitIds { get; set; } = null!;

    public int[] BadHabitIds { get; set; } = null!;

    public Tuple<int, byte>[] TreatIdUnitPerSessionPairs { get; set; } = null!;

    public string UserId { get; set; } = null!;
}
