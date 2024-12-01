using HTApp.Core.Contracts;
using HTApp.Infrastructure.EntityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HTApp.Web.MVC.Controllers;

public class TreatsController : Controller
{
    private IUnitOfWork repos;
    private UserManager<AppUser> userManager;
    public TreatsController(IUnitOfWork repos, UserManager<AppUser> userManager)
    {
        this.repos = repos;
        this.userManager = userManager;
    }
    
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Treats";
        var model = await repos.TreatRepository.GetAll(userManager.GetUserId(User)!);
        return View(model);
    }

    [HttpGet]
    public IActionResult AddTreat()
    {
        ViewData["Title"] = "Add Treat";
        var model = new TreatInputModel();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddTreat(TreatInputModel model)
    {
        ViewData["Title"] = "Add Treat";
        if(!ModelState.IsValid)
        {
            return View(model);
        }

        model.UserId = userManager.GetUserId(User)!;
        await repos.TreatRepository.Add(model);
        await repos.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> EditTreat(int id)
    {
        ViewData["Title"] = "Edit Treat";
        var model = await repos.TreatRepository.GetInputModel(id);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditTreat(TreatInputModel model, int id)
    {
        ViewData["Title"] = "Edit Treat";
        if(!ModelState.IsValid)
        {
            return View(model);
        }

        model.UserId = userManager.GetUserId(User)!;
        await repos.TreatRepository.Update(id, model);
        await repos.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> DeleteTreat(int id)
    {
        ViewData["Title"] = "Delete Treat";
        await repos.TreatRepository.Delete(id);
        await repos.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}
