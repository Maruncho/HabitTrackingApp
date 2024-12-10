using Microsoft.AspNetCore.Mvc;

namespace HTApp.Web.MVC.Controllers;

public class StatusCodeController : Controller
{
    public IActionResult Index(int? statusCode)
    {
        ViewData["Title"] = statusCode.ToString();
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
