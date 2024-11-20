namespace HTApp.Core.Contracts;

public class TreatModel<ModelId>
{
    public required ModelId Id { get; set; }

    public required string Name { get; set; }

    public byte QuantityPerSession { get; set; }

    public int Price { get; set; }
}
