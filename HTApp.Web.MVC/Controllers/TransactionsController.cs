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
        const int pageCount = 25;

        ITransactionService transactionService;
        UserManager<AppUser> userManager;

        public TransactionsController(ITransactionService transactionService, UserManager<AppUser> userManager)
        {
            this.transactionService = transactionService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(string? pageNumberParam, string? filterTypeName)
        {
            ViewData["Title"] = "Transactions";
            string userId = userManager.GetUserId(User)!;

            TransactionServiceResponse response = (await ParsePageNumberParameter(pageNumberParam, userId, filterTypeName ?? "")).Payload!;

            TransactionsViewModel viewModel = new TransactionsViewModel
            {
                PageCount = pageCount,
                HasNext = response.HasNext,
                PageNumber = response.PageNumber,
                PageBeginIndex = (response.PageNumber - 1) * pageCount + 1,
                FilterTypeName = filterTypeName ?? "",
                Models = response.Models,
                TypeNames = (await transactionService.GetTypeNames(userId)).Payload!,
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

        private async Task<Response<TransactionServiceResponse>> ParsePageNumberParameter(string? pageNumberParam, string userId, string? filterTypeName)
        {
            Response<TransactionServiceResponse> response;
            if(pageNumberParam is null)
            {
                response = await transactionService.GetAll(userId, pageCount, 1, filterTypeName ?? "");
            }
            else if(pageNumberParam == "last")
            {
                response = await transactionService.GetAllLatest(userId, pageCount);
            }
            else
            {
                bool res = int.TryParse(pageNumberParam, out int pageNumber);
                if (!res)
                {
                    response = await transactionService.GetAll(userId, pageCount, 1);
                }
                else
                {
                    response = await transactionService.GetAll(userId, pageCount, pageNumber);
                }
            }

            return response;
        }
    }
}
