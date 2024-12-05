namespace HTApp.Core.API;

 public class AppendCreditsResponse
{
    public int NewAmount { get; set; }
    public int Diff { get; set; }
    public bool Capped { get; set; }
}
