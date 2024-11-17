namespace HTApp.Core.Contracts;

public class TreatInputModel<UserIdType>
{
    public required string Name { get; set; }

    public byte QuantityPerSession { get; set; }

    public int Price { get; set; }

    public required UserIdType UserId { get; set; }
}
