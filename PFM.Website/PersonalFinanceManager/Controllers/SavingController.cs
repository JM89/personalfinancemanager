using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.Models.Saving;

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
        public ActionResult Index()
        {
            var accountId = GetCurrentAccount();

            AccountBasicInfo();

            var model = _savingService.GetSavingsByAccountId(accountId).OrderByDescending(x => x.DateSaving).ThenByDescending(x => x.Id).ToList();

            return View(model);
        }
        
        /// <summary>
        /// Initialize the Create form.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            AccountBasicInfo();

            var savingModel = new SavingEditModel();

            savingModel.DateSaving = DateTime.Today;

            PopulateDropDownLists(savingModel);

            return View(savingModel);
        }

        /// <summary>
        /// Create a new saving.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SavingEditModel savingEditModel)
        {
            if (ModelState.IsValid)
            {
                var accountId = GetCurrentAccount();
                savingEditModel.AccountId = accountId;

                _savingService.CreateSaving(savingEditModel);

                return RedirectToAction("Index");
            }

            return View(savingEditModel);
        }

        /// <summary>
        ///  Initialize the Edit form.
        /// </summary>
        /// <param name="id">Saving id</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            AccountBasicInfo();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var savingModel = _savingService.GetById(id.Value);
            PopulateDropDownLists(savingModel);

            if (savingModel == null)
            {
                return HttpNotFound();
            }

            return View(savingModel);
        }

        /// <summary>
        /// Update an existing saving.
        /// </summary>
        /// <param name="savingEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SavingEditModel savingEditModel)
        {
            PopulateDropDownLists(savingEditModel);

            if (ModelState.IsValid)
            {
                var accountId = GetCurrentAccount();
                savingEditModel.AccountId = accountId;

                _savingService.EditSaving(savingEditModel);
                
                return RedirectToAction("Index");
            }
            return View(savingEditModel);
        }

        private void PopulateDropDownLists(SavingEditModel savingModel)
        {
            savingModel.AvailableInternalAccounts = _bankAccountService.GetAccountsByUser(CurrentUser)
                .Where(x => x.Id != GetCurrentAccount() && x.IsSavingAccount)
                .Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
        }

        /// <summary>
        /// Delete the ATM withdraw after confirmation.
        /// </summary>
        /// <param name="id">ATM withdraw id</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _savingService.DeleteSaving(id);

            return Content(Url.Action("Index"));
        }        
    }
}
