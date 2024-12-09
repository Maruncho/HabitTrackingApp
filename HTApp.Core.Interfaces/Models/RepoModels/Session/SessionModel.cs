namespace HTApp.Core.API;

public class SessionModel
{
    public required int Id { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public byte Refunds { get; set; }

    public required SessionGoodHabitModel[] GoodHabits { get; set; }

    public required SessionBadHabitModel[] BadHabits { get; set; }

    //change of plans
    //public required TransactionModel[] Transactions { get; set; }

    public required SessionTreatModel[] Treats { get; set; }
}
