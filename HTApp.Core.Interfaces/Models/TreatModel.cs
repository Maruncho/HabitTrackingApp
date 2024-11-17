namespace HTApp.Core.Contracts;

public class TreatModel
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public byte QuantityPerSession { get; set; }

    public int Price { get; set; }
}
