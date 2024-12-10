using HTApp.Web.MVC.CustomValidation;

namespace HTApp.Web.MVC.Areas.Admin.Models;

public class ChangeBGColorForm
{
    [ColorHex]
    public string BGColor { get; set; } = "#FFFFFF";
}
