using HTApp.Core.API;
using HTApp.Infrastructure.EntityModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HTApp.Web.MVC.Controllers;

[Authorize]
public class TreatsController : Controller
{
    private ITreatService treatService;
    private UserManager<AppUser> userManager;
    public TreatsController(ITreatService treatService, UserManager<AppUser> userManager)
    {
        this.treatService = treatService;
        this.userManager = userManager;
    }
    
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Treats";
        var model = (await treatService.GetAll(userManager.GetUserId(User)!)).Payload;
        return View(model);
    }

    [HttpGet]
    public IActionResult AddTreat()
    {
        ViewData["Title"] = "Add Treat";
        var model = new TreatFormModel();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddTreat(TreatFormModel model)
    {
        ViewData["Title"] = "Add Treat";
        if(!ModelState.IsValid)
        {
            return View(model);
        }

        string userId = userManager.GetUserId(User)!;
        Response response = await treatService.Add(model, userId);

        switch(response.Code)
        {
            case ResponseCode.InvalidField:
            case ResponseCode.RepositoryError:
                ViewData["ErrorMessage"] = response.Message;
                return View(model);
            case ResponseCode.Success:
                return RedirectToAction("Index");
            default:
                throw new Exception("Unhandled ResponseCode");
        }
    }

    [HttpGet]
    public async Task<IActionResult> EditTreat(int id)
    {
        ViewData["Title"] = "Edit Treat";
        string userId = userManager.GetUserId(User)!;
        Response<TreatInputModel> response = await treatService.GetInputModel(id, userId);

        switch(response.Code)
        {
            case ResponseCode.NotFound:
                return NotFound();
            case ResponseCode.Unauthorized:
                return Unauthorized();
            case ResponseCode.Success:
                return View((TreatFormModel)response.Payload!);
            default:
                throw new Exception("Unhandled ResponseCode");
        }
    }

    [HttpPut]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditTreat(TreatFormModel model, int id)
    {
        ViewData["Title"] = "Edit Treat";
        if(!ModelState.IsValid)
        {
            return View(model);
        }

        string userId = userManager.GetUserId(User)!;
        Response response = await treatService.Update(id, model, userId);

        switch (response.Code)
        {
            case ResponseCode.InvalidField:
                ViewData["ErrorMessage"] = response.Message;
                return View(model);
            case ResponseCode.NotFound:
                return NotFound();
            case ResponseCode.Unauthorized:
                return Unauthorized();
            case ResponseCode.RepositoryError:
                ViewData["ErrorMessage"] = response.Message;
                return View(model);
            case ResponseCode.Success:
                return RedirectToAction("Index");
            default:
                throw new Exception("Unhandled ResponseCode");
        }
    }

    [HttpDelete]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteTreat(int id)
    {
        ViewData["Title"] = "Delete Treat";
        string userId = userManager.GetUserId(User)!;
        Response response = await treatService.Delete(id, userId);

        switch(response.Code)
        {
            case ResponseCode.NotFound:
                return NotFound();
            case ResponseCode.Unauthorized:
                return Unauthorized();
            case ResponseCode.Success:
                return RedirectToAction("Index");
            default:
                throw new Exception("Unhandled ResponseCode");
        }
    }
}
