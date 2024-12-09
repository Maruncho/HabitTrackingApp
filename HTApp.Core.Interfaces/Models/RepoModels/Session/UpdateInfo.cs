namespace HTApp.Core.API;

public class UpdateInfo
{
    public bool Success { get; set; }

    public int[] Added { get; set; } = [];

    public int[] Removed { get; set; } = [];
}
