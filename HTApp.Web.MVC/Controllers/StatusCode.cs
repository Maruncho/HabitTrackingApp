using Microsoft.AspNetCore.Mvc;

namespace HTApp.Web.MVC.Controllers;

public class StatusCode : Controller
{
    public IActionResult Index(int? statusCode)
    {
        switch(statusCode)
        {
            case 404:
                return View("404");
            case 500:
                return View("500");
            default:
                return RedirectToAction("Index", "Home");
        }
    }
}
