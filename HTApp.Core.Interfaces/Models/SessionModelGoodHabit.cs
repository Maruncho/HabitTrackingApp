namespace HTApp.Core.Contracts;

public class SessionModelGoodHabit<ModelId>
{
    public required ModelId Id { get; set; }

    public required string Name { get; set; }

    public bool Completed { get; set; }
}
