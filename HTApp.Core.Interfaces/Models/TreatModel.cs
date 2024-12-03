namespace HTApp.Core.API;

public class TreatModel
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    public byte QuantityPerSession { get; set; }

    public int Price { get; set; }
}
