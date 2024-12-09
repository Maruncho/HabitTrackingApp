using HTApp.Core.API;
using HTApp.Infrastructure.EntityModels;
using HTApp.Web.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HTApp.Web.MVC.Controllers;

[Authorize]
public class SessionController : Controller
{

    ISessionService sessionService;
    IUserDataService userDataService;
    UserManager<AppUser> userManager;

    public SessionController(ISessionService sessionService, IUserDataService userDataService, UserManager<AppUser> userManager)
    {
        this.sessionService = sessionService;
        this.userDataService = userDataService;
        this.userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Session";
        if (TempData.TryGetValue("Error", out var error))
        {
            ViewData["Error"] = error;
        }

        string userId = userManager.GetUserId(User)!;

        int userCredits = (await userDataService.GetCredits(userId)).Payload!;

        var sessionResponse = await sessionService.GetLastSession(userId, true);

        SessionModel? session = null;
        if (sessionResponse.Code == ResponseCode.Success)
        {
            session = sessionResponse.Payload!;
        }

        SessionViewModel viewModel = new SessionViewModel
        {
            UserCredits = userCredits,
            SessionModel = session,
        };

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Refresh()
    {
        string userId = userManager.GetUserId(User)!;

        var res = await sessionService.RefreshIfDataIsNotInSync(userId);
        if (res.Code != ResponseCode.Success)
        {
            throw new Exception("Unhandled response");
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> StartNewSession()
    {
        string userId = userManager.GetUserId(User)!;

        var response = await sessionService.StartNewSession(userId);

        switch (response.Code)
        {
            case ResponseCode.InvalidOperation:
                TempData["Error"] = "Already in Session. Finish the current first.";
                return RedirectToAction("Index");
            case ResponseCode.RepositoryError:
                TempData["Error"] = "Couldn't start new session. Please try again.";
                return RedirectToAction("Index");
            case ResponseCode.Success:
                return RedirectToAction("Index");
            default:
                throw new Exception("Unhandled Response.");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FinishSession()
    {
        string userId = userManager.GetUserId(User)!;

        var response = await sessionService.FinishCurrentSession(userId);

        switch (response.Code)
        {
            case ResponseCode.InvalidOperation:
                TempData["Error"] = response.Message;
                return RedirectToAction("Index");
            case ResponseCode.RepositoryError:
            case ResponseCode.ServiceError:
                TempData["Error"] = "Couldn't finish session. Please try again.";
                return RedirectToAction("Index");
            case ResponseCode.Success:
                return RedirectToAction("Index");
            default:
                throw new Exception("Unhandled Response.");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateGoodHabit(int ghId, int success)
    {
        string userId = userManager.GetUserId(User)!;

        var response = await sessionService.UpdateGoodHabit(ghId, success != 0, userId);

        switch (response.Code)
        {
            case ResponseCode.InvalidOperation:
                TempData["Error"] = "Not currently in session.";
                return RedirectToAction("Index");
            case ResponseCode.RepositoryError:
            case ResponseCode.ServiceError: //spammy, but f&%! it!
                TempData["Error"] = response.Message;
                return RedirectToAction("Index");
            case ResponseCode.NotFound:
                TempData["Error"] = "GoodHabit not in session. Maybe data is not in sync. You can refresh";
                return RedirectToAction("Index");
            case ResponseCode.Success:
                return RedirectToAction("Index");
            default:
                throw new Exception("Unhandled Response.");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateBadHabit(int bhId, int fail)
    {
        string userId = userManager.GetUserId(User)!;

        var response = await sessionService.UpdateBadHabit(bhId, fail != 0, userId);

        switch (response.Code)
        {
            case ResponseCode.InvalidOperation:
                TempData["Error"] = "Not currently in session.";
                return RedirectToAction("Index");
            case ResponseCode.RepositoryError:
            case ResponseCode.ServiceError: //spammy, but f&%! it!
                TempData["Error"] = response.Message;
                return RedirectToAction("Index");
            case ResponseCode.NotFound:
                TempData["Error"] = "BadHabit not in session. Maybe data is not in sync. You can refresh";
                return RedirectToAction("Index");
            case ResponseCode.Success:
                return RedirectToAction("Index");
            default:
                throw new Exception("Unhandled Response.");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BuyTreat(int trId)
    {
        string userId = userManager.GetUserId(User)!;

        var response = await sessionService.BuyTreat(trId, userId);

        switch (response.Code)
        {
            case ResponseCode.InvalidOperation:
                TempData["Error"] = response.Message;
                return RedirectToAction("Index");
            case ResponseCode.RepositoryError:
            case ResponseCode.ServiceError: //spammy, but f&%! it!
                TempData["Error"] = response.Message;
                return RedirectToAction("Index");
            case ResponseCode.NotFound:
                TempData["Error"] = "Treat not in session. Maybe data is not in sync. You can refresh";
                return RedirectToAction("Index");
            case ResponseCode.Success:
                return RedirectToAction("Index");
            default:
                throw new Exception("Unhandled Response.");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RefundTreat(int trId)
    {
        string userId = userManager.GetUserId(User)!;

        var response = await sessionService.RefundTreat(trId, userId);

        switch (response.Code)
        {
            case ResponseCode.InvalidOperation:
                TempData["Error"] = response.Message;
                return RedirectToAction("Index");
            case ResponseCode.RepositoryError:
            case ResponseCode.ServiceError: //spammy, but f&%! it!
                TempData["Error"] = response.Message;
                return RedirectToAction("Index");
            case ResponseCode.NotFound:
                TempData["Error"] = "Treat not in session. Maybe data is not in sync. You can refresh";
                return RedirectToAction("Index");
            case ResponseCode.Success:
                return RedirectToAction("Index");
            default:
                throw new Exception("Unhandled Response.");
        }
    }
}
