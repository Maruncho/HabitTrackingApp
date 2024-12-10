using HTApp.Infrastructure.EntityModels;
using HTApp.Web.MVC.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HTApp.Web.MVC.Areas.Admin.Controllers;

[Authorize(Roles = "Admin")]
[Area("Admin")]
public class DashboardController(UserManager<AppUser> userManager/*, RoleManager<IdentityRole> roleManager*/) : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Admin Dashboard";
        return View();
    }

    [HttpGet]
    public IActionResult AddAdmin()
    {
        ViewData["Title"] = "Add new Admin";
        return View(new AddAdminFormModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddAdminAsync(AddAdminFormModel model)
    {
        ViewData["Title"] = "Add new Admin";
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await userManager.FindByIdAsync(model.UserId);
        if (user is null)
        {
            ModelState.AddModelError("UserId", "Couldn't find a user with this Id");
            return View(model);
        }

        bool already = await userManager.IsInRoleAsync(user, "Admin");
        if (already)
        {
            ModelState.AddModelError("UserId", "User is already an admin.");
            return View(model);
        }

        IdentityResult result = await userManager.AddToRoleAsync(user, "Admin");
        if (!result.Succeeded)
        {
            ModelState.AddModelError("UserId", "Internal Database error. Couldn't add admin. Try again.");
            return View(model);
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult ChangeBGColor()
    {
        return View(new ChangeBGColorForm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ChangeBGColor(ChangeBGColorForm model)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        Global.BACKGROUND_COLOR = model.BGColor;

        return RedirectToAction("Index");
    }
}
