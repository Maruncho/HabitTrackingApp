namespace HTApp.Core.API;

public class SessionGoodHabitModel
{
    public int Id { get; set; }
    public string Label { get; set; } = null!;
    public bool Completed { get; set; }
}
