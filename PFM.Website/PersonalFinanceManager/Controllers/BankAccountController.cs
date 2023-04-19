using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PersonalFinanceManager.Models.Account;
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class BankAccountController : BaseController
    {
        private readonly IBankAccountService _bankAccountService;
        private readonly ICurrencyService _currencyService;
        private readonly IBankService _bankService;

        public BankAccountController(IBankAccountService bankAccountService, ICurrencyService currencyService, IBankService bankService, Serilog.ILogger logger) : base(bankAccountService, logger)
        {
            this._bankAccountService = bankAccountService;
            this._currencyService = currencyService;
            this._bankService = bankService;
        }

        /// <summary>
        /// Return the list of accounts of an authenticated user as a Json object, for the menu. 
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetAccounts()
        {
            var accountsMenu = await _bankAccountService.GetAccountsByUser(User.Identity.GetUserId());

            if (!accountsMenu.Any())
            {
                return Json(new List<AccountListModel>(), JsonRequestBehavior.AllowGet);
            }
            
            var first = accountsMenu.First(x => x.IsFavorite);
            var otherAccounts = accountsMenu.Where(x => !x.IsFavorite).OrderBy(x => x.BankName);

            var generatedAccountMenu = new List<AccountListModel>();
            generatedAccountMenu.Add(first);
            generatedAccountMenu.AddRange(otherAccounts);

            return Json(generatedAccountMenu, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Return the list of accounts of an authenticated user.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var model = (await _bankAccountService.GetAccountsByUser(User.Identity.GetUserId())).OrderBy(x => x.Name);

            return View(model);
        }

        /// <summary>
        /// Populate the list of currencies and banks for the Create / Edit form. 
        /// </summary>
        /// <param name="accountModel"></param>
        private async Task PopulateDropDownLists(AccountEditModel accountModel)
        {
            accountModel.AvailableCurrencies = (await _currencyService.GetCurrencies()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
            accountModel.AvailableBanks = (await _bankService.GetBanks()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
        }

        /// <summary>
        /// Initialize the Create form.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Create()
        {
            var accountModel = new AccountEditModel();

            await PopulateDropDownLists(accountModel);

            return View(accountModel);
        }

        /// <summary>
        /// Create a new account.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AccountEditModel accountEditModel)
        {
            if (ModelState.IsValid)
            {
                await _bankAccountService.CreateBankAccount(accountEditModel, User.Identity.GetUserId());

                return RedirectToAction("Index");
            }

            await PopulateDropDownLists(accountEditModel);

            return View(accountEditModel);
        }

        /// <summary>
        ///  Initialize the Edit form.
        /// </summary>
        /// <param name="id">Account id</param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var accountModel = await _bankAccountService.GetById(id.Value);
            
            if (accountModel == null)
            {
                return HttpNotFound();
            }

            await PopulateDropDownLists(accountModel);

            return View(accountModel);
        }

        /// <summary>
        /// Update an existing account.
        /// </summary>
        /// <param name="accountEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AccountEditModel accountEditModel)
        {
            if (ModelState.IsValid)
            {
                await _bankAccountService.EditBankAccount(accountEditModel, User.Identity.GetUserId());
                
                return RedirectToAction("Index");
            }
            return View(accountEditModel);
        }

        /// <summary>
        /// Delete the account after confirmation.
        /// </summary>
        /// <param name="id">Account id</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _bankAccountService.DeleteBankAccount(id);

            return Content(Url.Action("Index"));
        }

        public async Task<ActionResult> SetAsFavorite(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            await _bankAccountService.SetAsFavorite(id.Value);

            return RedirectToAction("Index");
        }
    }
}
