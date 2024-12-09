using HTApp.Core.API;

namespace HTApp.Web.MVC.Models;

public class SessionViewModel
{
    public int UserCredits { get; set; }

    public SessionModel? SessionModel { get; set; }
}
