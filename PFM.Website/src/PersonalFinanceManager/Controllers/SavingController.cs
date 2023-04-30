using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.Models.Saving;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class SavingController : BaseController
    {
        private readonly ISavingService _savingService;
        private readonly IBankAccountService _bankAccountService;

        public SavingController(ISavingService savingService, IBankAccountService bankAccountService, Serilog.ILogger logger) : base(bankAccountService, logger)
        {
            this._savingService = savingService;
            this._bankAccountService = bankAccountService;
        }

        /// <summary>
        /// Return the list of ATM withdraws.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var accountId = await GetCurrentAccount();

            await AccountBasicInfo();

            var model = (await _savingService.GetSavingsByAccountId(accountId)).OrderByDescending(x => x.DateSaving).ThenByDescending(x => x.Id).ToList();

            return View(model);
        }
        
        /// <summary>
        /// Initialize the Create form.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Create()
        {
            await AccountBasicInfo();

            var savingModel = new SavingEditModel();

            savingModel.DateSaving = DateTime.Today;

            await PopulateDropDownLists(savingModel);

            return View(savingModel);
        }

        /// <summary>
        /// Create a new saving.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SavingEditModel savingEditModel)
        {
            if (ModelState.IsValid)
            {
                var accountId = await GetCurrentAccount();
                savingEditModel.AccountId = accountId;

                await _savingService.CreateSaving(savingEditModel);

                return RedirectToAction("Index");
            }

            return View(savingEditModel);
        }

        private async Task PopulateDropDownLists(SavingEditModel savingModel)
        {
            var accountId = await GetCurrentAccount();

            savingModel.AvailableInternalAccounts = (await _bankAccountService.GetAccountsByUser(CurrentUser))
                .Where(x => x.Id != accountId && x.IsSavingAccount)
                .Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
        }

        /// <summary>
        /// Delete the ATM withdraw after confirmation.
        /// </summary>
        /// <param name="id">ATM withdraw id</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _savingService.DeleteSaving(id);

            return Content(Url.Action("Index"));
        }        
    }
}
