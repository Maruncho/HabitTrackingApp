using HTApp.Core.Contracts;
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
        private IUnitOfWork repos;
        private UserManager<AppUser> userManager;
        public HabitsController(IUnitOfWork repos, UserManager<AppUser> userManager)
        {
            this.repos = repos;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Habits";
            HabitsViewModel model = new HabitsViewModel { BadHabits = [], GoodHabits = [] };
            model.GoodHabits = await repos.GoodHabitRepository.GetAll(userManager.GetUserId(User)!);
            model.BadHabits = await repos.BadHabitRepository.GetAll(userManager.GetUserId(User)!);
            return View(model);
        }

        [HttpGet]
        public IActionResult AddGoodHabit()
        {
            ViewData["Title"] = "Add Good Habit";
            var model = new GoodHabitInputModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddGoodHabit(GoodHabitInputModel model)
        {
            ViewData["Title"] = "Add Good Habit";
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            model.UserId = userManager.GetUserId(User)!;
            await repos.GoodHabitRepository.Add(model);
            await repos.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddBadHabit()
        {
            ViewData["Title"] = "Add Bad Habit";
            var model = new BadHabitInputModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddBadHabit(BadHabitInputModel model)
        {
            ViewData["Title"] = "Add Bad Habit";
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            model.UserId = userManager.GetUserId(User)!;
            await repos.BadHabitRepository.Add(model);
            await repos.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditGoodHabitAsync(int id)
        {
            ViewData["Title"] = "Edit Good Habit";
            var model = await repos.GoodHabitRepository.GetInputModel(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditGoodHabit(GoodHabitInputModel model, int id)
        {
            ViewData["Title"] = "Edit Good Habit";
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            model.UserId = userManager.GetUserId(User)!;
            await repos.GoodHabitRepository.Update(id, model);
            await repos.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditBadHabit(int id)
        {
            ViewData["Title"] = "Edit Bad Habit";
            var model = await repos.BadHabitRepository.GetInputModel(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditBadHabit(BadHabitInputModel model, int id)
        {
            ViewData["Title"] = "Edit Bad Habit";
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            model.UserId = userManager.GetUserId(User)!;
            await repos.BadHabitRepository.Update(id, model);
            await repos.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteGoodHabit(int id)
        {
            ViewData["Title"] = "Delete Good Habit";
            await repos.GoodHabitRepository.Delete(id);
            await repos.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteBadHabit(int id)
        {
            ViewData["Title"] = "Delete Good Habit";
            await repos.BadHabitRepository.Delete(id);
            await repos.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
