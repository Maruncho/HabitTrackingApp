using HTApp.Core.API;
using HTApp.Infrastructure.EntityModels;
using HTApp.Web.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HTApp.Web.MVC.Controllers
{
    [Authorize]
    public class HabitsController : Controller
    {
        private IGoodHabitService ghService;
        private IBadHabitService bhService;
        private UserManager<AppUser> userManager;
        public HabitsController(IGoodHabitService ghService, IBadHabitService bhService, UserManager<AppUser> userManager)
        {
            this.ghService = ghService;
            this.bhService = bhService;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Habits";
            HabitsViewModel model = new HabitsViewModel { BadHabits = [], GoodHabits = [] };
            model.GoodHabits = (await ghService.GetAll(userManager.GetUserId(User)!)).Payload!;
            model.BadHabits = (await bhService.GetAll(userManager.GetUserId(User)!)).Payload!;

            //easily get userid for admin adding (DEVELOPMENT/TESTING ONLY)
            //Console.WriteLine("///////////////////////////////////////////////////// " + userManager.GetUserId(User));

            return View(model);

        }

        [HttpGet]
        public IActionResult AddGoodHabit()
        {
            ViewData["Title"] = "Add Good Habit";
            var model = new GoodHabitFormModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGoodHabit(GoodHabitFormModel model)
        {
            ViewData["Title"] = "Add Good Habit";
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            string userId = userManager.GetUserId(User)!;
            Response response = await ghService.Add(model, userId);

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
        public IActionResult AddBadHabit()
        {
            ViewData["Title"] = "Add Bad Habit";
            var model = new BadHabitFormModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBadHabit(BadHabitFormModel model)
        {
            ViewData["Title"] = "Add Bad Habit";
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            string userId = userManager.GetUserId(User)!;
            Response response = await bhService.Add(model, userId);

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
        public async Task<IActionResult> EditGoodHabit(int id)
        {
            ViewData["Title"] = "Edit Good Habit";
            string userId = userManager.GetUserId(User)!;
            Response<GoodHabitInputModel> response = await ghService.GetInputModel(id, userId);

            switch(response.Code)
            {
                case ResponseCode.NotFound:
                    return NotFound();
                case ResponseCode.Unauthorized:
                    return Unauthorized();
                case ResponseCode.Success:
                    return View((GoodHabitFormModel)response.Payload!); //sadly the views do not implicitly convert
                default:
                    throw new Exception("Unhandled ResponseCode");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGoodHabit(GoodHabitFormModel model, int id)
        {
            ViewData["Title"] = "Edit Good Habit";
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            string userId = userManager.GetUserId(User)!;
            Response response = await ghService.Update(id, model, userId);

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

        [HttpGet]
        public async Task<IActionResult> EditBadHabit(int id)
        {
            ViewData["Title"] = "Edit Bad Habit";
            string userId = userManager.GetUserId(User)!;
            Response<BadHabitInputModel> response = await bhService.GetInputModel(id, userId);

            switch(response.Code)
            {
                case ResponseCode.NotFound:
                    return NotFound();
                case ResponseCode.Unauthorized:
                    return Unauthorized();
                case ResponseCode.Success:
                    return View((BadHabitFormModel)response.Payload!);
                default:
                    throw new Exception("Unhandled ResponseCode");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBadHabit(BadHabitFormModel model, int id)
        {
            ViewData["Title"] = "Edit Bad Habit";
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            string userId = userManager.GetUserId(User)!;
            Response response = await bhService.Update(id, model, userId);

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

        [HttpGet]
        public async Task<IActionResult> DeleteGoodHabit(int id)
        {
            ViewData["Title"] = "Delete Good Habit";
            string userId = userManager.GetUserId(User)!;
            Response response = await ghService.Delete(id, userId);

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

        [HttpGet]
        public async Task<IActionResult> DeleteBadHabit(int id)
        {
            ViewData["Title"] = "Delete Good Habit";
            string userId = userManager.GetUserId(User)!;
            Response response = await bhService.Delete(id, userId);

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
}
