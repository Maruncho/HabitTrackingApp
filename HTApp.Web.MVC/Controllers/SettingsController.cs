using HTApp.Core.API;
using HTApp.Infrastructure.EntityModels;
using HTApp.Web.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HTApp.Web.MVC.Controllers;

[Authorize]
public class SettingsController : Controller
{

    IUserDataService userDataService;
    UserManager<AppUser> userManager;

    public SettingsController(IUserDataService userDataService, UserManager<AppUser> userManager)
    {
        this.userDataService = userDataService;
        this.userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Settings";
        string userId = userManager.GetUserId(User)!;

        var data = (await userDataService.GetUserData(userId)).Payload!;

        var model = new SettingsForm
        {
            RefundsPerSession = data.RefundsPerSession
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(SettingsForm model)
    {
        ViewData["Title"] = "Settings";
        string userId = userManager.GetUserId(User)!;

        if(!ModelState.IsValid)
        {
            return View(model);
        }

        await userDataService.SetRefundsPerSession(model.RefundsPerSession, userId);

        ViewData["Success"] = "Success!";
        return View(model);
    }
}
