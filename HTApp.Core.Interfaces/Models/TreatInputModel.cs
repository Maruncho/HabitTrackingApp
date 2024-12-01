namespace HTApp.Core.Contracts;

public class TreatInputModel
{
    public required string Name { get; set; }

    public byte QuantityPerSession { get; set; }

    public int Price { get; set; }

    public required string UserId { get; set; }
}
