namespace HTApp.Core.Contracts;

public class TreatInputModel<UserIdType> : TreatModel
{
    public required UserIdType UserId { get; set; }
}
