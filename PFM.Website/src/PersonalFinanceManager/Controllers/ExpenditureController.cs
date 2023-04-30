using PersonalFinanceManager.Models.Expenditure;
using PersonalFinanceManager.Services.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PersonalFinanceManager.Controllers
{
    [Authorize]
    public class ExpenditureController : BaseController
    {
        private readonly IExpenditureService _expenditureService;
        private readonly IExpenditureTypeService _expenditureTypeService;
        private readonly IBankAccountService _bankAccountService;
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly IAtmWithdrawService _atmWithdrawService;

        public ExpenditureController(IExpenditureService expenditureService, IExpenditureTypeService expenditureTypeService, IBankAccountService bankAccountService,
            IPaymentMethodService paymentMethodService, IAtmWithdrawService atmWithdrawService, Serilog.ILogger logger) : base(bankAccountService, logger)
        {
            this._bankAccountService = bankAccountService;
            this._expenditureService = expenditureService;
            this._expenditureTypeService = expenditureTypeService;
            this._paymentMethodService = paymentMethodService;
            this._atmWithdrawService = atmWithdrawService;
        }

        // GET: ExpenditureModels
        public async Task<ActionResult> Index()
        {
            await AccountBasicInfo();

            var expenditures = (await _expenditureService.GetExpenditures(new Models.SearchParameters.ExpenditureGetListSearchParameters() { AccountId = await GetCurrentAccount() }))
                .OrderByDescending(x => x.DateExpenditure)
                .ThenByDescending(x => x.Id)
                .ToList();

            return View(expenditures);
        }

        private async Task PopulateDropDownLists(ExpenditureEditModel expenditureModel)
        {
            var currentAccount = await GetCurrentAccount();

            expenditureModel.AvailableInternalAccounts = (await _bankAccountService.GetAccountsByUser(CurrentUser)).Where(x => x.Id != currentAccount).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
            expenditureModel.AvailableExpenditureTypes = (await _expenditureTypeService.GetExpenditureTypes()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).OrderBy(x => x.Text).ToList();
            expenditureModel.AvailablePaymentMethods = (await _paymentMethodService.GetPaymentMethods()).ToList();
            expenditureModel.AvailableAtmWithdraws = (await _atmWithdrawService.GetAtmWithdrawsByAccountId(currentAccount)).Where(x => !x.IsClosed).OrderBy(x => x.DateExpenditure).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Description }).ToList();
        }

        // GET: ExpenditureModels/Create
        public async Task<ActionResult> Create(DateTime? dateLastExpenditure = null)
        {
            await AccountBasicInfo();

            ExpenditureEditModel expenditureModel = new ExpenditureEditModel
            {
                DateExpenditure = dateLastExpenditure ?? DateTime.Today
            };
            
            await PopulateDropDownLists(expenditureModel);

            return View(expenditureModel);
        }

        // POST: ExpenditureModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ExpenditureEditModel expenditureModel, bool stayHere)
        {
            if (ModelState.IsValid)
            {
                var accountId = await GetCurrentAccount();

                expenditureModel.AccountId = accountId;
                await _expenditureService.CreateExpenditure(expenditureModel);

                if (stayHere)
                {
                    return RedirectToAction("Create", new { DateLastExpenditure = expenditureModel.DateExpenditure });
                }

                return RedirectToAction("Index", new { accountId });
            }
            else
            {
                await PopulateDropDownLists(expenditureModel);
            }

            return View(expenditureModel);
        }

        public async Task<ActionResult> UndoDebit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            await _expenditureService.ChangeDebitStatus(id.Value, false);

            var accountId = await GetCurrentAccount();

            return RedirectToAction("Index", new { accountId });
        }

        public async Task<ActionResult> Debit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            await _expenditureService.ChangeDebitStatus(id.Value, true);

            var accountId = await GetCurrentAccount();

            return RedirectToAction("Index", new { accountId });
        }

        // POST: ExpenditureModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _expenditureService.DeleteExpenditure(id);

            return Content(Url.Action("Index"));
        }
    }
}
