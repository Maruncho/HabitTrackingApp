namespace HTApp.Core.Contracts;

public class SessionModel<ModelId, GoodHabitModel, BadHabitModel, TreatModel, TransactionModel>
{
    public required ModelId Id { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndData { get; set; }

    public byte Refunds { get; set; }


}
