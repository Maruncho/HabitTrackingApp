namespace HTApp.Core.API;

public class SessionTreatModel
{
    public int Id { get; set; }
    public string Label { get; set; } = null!;
    public byte UnitsLeft { get; set; }
    public byte UnitsBought { get; set; }
}
