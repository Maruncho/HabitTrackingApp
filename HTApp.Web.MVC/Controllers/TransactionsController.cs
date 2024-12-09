using HTApp.Core.API;
using HTApp.Infrastructure.EntityModels;
using HTApp.Web.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HTApp.Web.MVC.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        const int pageCount = 20;

        ITransactionService transactionService;
        IUserDataService userDataService;
        UserManager<AppUser> userManager;

        public TransactionsController(ITransactionService transactionService, IUserDataService userDataService, UserManager<AppUser> userManager)
        {
            this.transactionService = transactionService;
            this.userDataService = userDataService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(string? pageNumberParam, string? filterTypeName, bool? fromLastSession)
        {
            ViewData["Title"] = "Transactions";
            string userId = userManager.GetUserId(User)!;

            TransactionServiceResponse response = (await ParsePageNumberParameter(pageNumberParam, userId, filterTypeName ?? "", fromLastSession ?? false)).Payload!;

            int userCredits = (await userDataService.GetCredits(userId)).Payload!;

            TransactionsViewModel viewModel = new TransactionsViewModel
            {
                UserCredits = userCredits,
                PageCount = pageCount,
                HasNext = response.HasNext,
                PageNumber = response.PageNumber,
                PageBeginIndex = (response.PageNumber - 1) * pageCount + 1,
                FilterTypeName = filterTypeName ?? "",
                FromLastSession = fromLastSession ?? false,
                Models = response.Models,
                TypeNames = (await transactionService.GetTypeNames(userId, filterTypeName ?? "", fromLastSession ?? false)).Payload!,
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult AddManual()
        {
            ViewData["Title"] = "Add Manual Transaction";
            var model = new TransactionAddManualFormModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddManual(TransactionAddManualFormModel model)
        {
            ViewData["Title"] = "Add Manual Transaction";

            if(!ModelState.IsValid)
            {
                return View(model);
            }

            string userId = userManager.GetUserId(User)!;
            Response response = await transactionService.AddManual(model.Amount, userId);

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

        private async Task<Response<TransactionServiceResponse>> ParsePageNumberParameter(string? pageNumberParam, string userId, string? filterTypeName, bool? fromLastSession)
        {
            if(pageNumberParam == "last")
            {
                return await transactionService.GetAllLatest(userId, pageCount, filterTypeName ?? "", fromLastSession ?? false);
            }

            //handles null too
            bool res = int.TryParse(pageNumberParam, out int pageNumber);
            if (!res)
            {
                pageNumber = 1;
            }

            return await transactionService.GetAll(userId, pageCount, pageNumber, filterTypeName ?? "", fromLastSession ?? false);
        }
    }
}
