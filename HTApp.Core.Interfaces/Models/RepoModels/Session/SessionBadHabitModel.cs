namespace HTApp.Core.API;

public class SessionBadHabitModel
{
    public int Id { get; set; }
    public string Label { get; set; } = null!;
    public bool Failed { get; set; }
}
