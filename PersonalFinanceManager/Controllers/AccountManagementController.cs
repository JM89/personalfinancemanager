using System.Linq;
using System.Web.Mvc;
using PersonalFinanceManager.Models.AccountManagement;
using PersonalFinanceManager.Models.SearchParameters;
using PersonalFinanceManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using PersonalFinanceManager.Utils.Helpers;
using WebGrease.Css.Extensions;

namespace PersonalFinanceManager.Controllers
{
    public class AccountManagementController : BaseController
    {
        private readonly IExpenditureService _expenditureService;
        private readonly IIncomeService _incomeService;
        private readonly IAtmWithdrawService _atmWithdrawService;
        private readonly IExpenditureTypeService _expenditureTypeService;
        private readonly IBudgetPlanService _budgetPlanService;
        private readonly IBankAccountService _bankAccountService;
        private readonly IPaymentMethodService _paymentMethodService;

        public AccountManagementController(IExpenditureService expenditureService, IBankAccountService bankAccountService, IBudgetPlanService budgetPlanService, IExpenditureTypeService expenditureTypeService, 
            IPaymentMethodService paymentMethodService, IIncomeService incomeService, IAtmWithdrawService atmWithdrawService) : base(bankAccountService)
        {
            this._budgetPlanService = budgetPlanService;
            this._expenditureService = expenditureService;
            this._expenditureTypeService = expenditureTypeService;
            this._bankAccountService = bankAccountService;
            this._paymentMethodService = paymentMethodService;
            this._incomeService = incomeService;
            this._atmWithdrawService = atmWithdrawService;
        }

        public ActionResult Index()
        {
            if (!HasAccount)
            {
                return Redirect("/BankAccount/Index");
            }

            // Get current budget plan if it exists
            var budgetPlan = _budgetPlanService.GetCurrent(GetCurrentAccount());

            // Get the expense summary by category
            var expenditureSummaryModel = _expenditureService.GetExpenseSummary(GetCurrentAccount(), budgetPlan);

            return View(expenditureSummaryModel);
        }


        public JsonResult SaveCurrentAccount(int accountId, int indexAccountList)
        {
            Session["PreviousAccount"] = Session["CurrentAccount"];
            Session["CurrentAccount"] = accountId;
            Session["IndexAccountList"] = indexAccountList;

            if (Session["PreviousAccount"] == null || (int)Session["PreviousAccount"] == (int)Session["CurrentAccount"])
            {
                Session["ReloadPage"] = false;
            }
            else
            {
                Session["ReloadPage"] = true;
            }

            return Json(new
            {
                Data = new
                {
                    accountId = accountId,
                    reloadPage = Session["ReloadPage"]
                }
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowImportMovements()
        {
            var expenseTypes = _expenditureTypeService.GetExpenditureTypes();
            var currentAccount = _bankAccountService.GetById(GetCurrentAccount());
            var lastMovement = _expenditureService.GetExpenditures(new ExpenditureGetListSearchParameters() { AccountId = currentAccount.Id }).OrderByDescending(x => x.DateExpenditure).FirstOrDefault();
            var paymentMethods = _paymentMethodService.GetPaymentMethods();
            var importTypes = Enum.GetValues(typeof(ImportTypes)).Cast<ImportTypes>();

            var model = new ImportMovementModel
            {
                AccountId = currentAccount.Id,
                AccountCurrentBalance = currentAccount.CurrentBalance,
                AccountCurrencySymbol = currentAccount.CurrencySymbol,
                ExpenseTypes = expenseTypes.Select(x => new SelectListItem() {Text = x.Name, Value = x.Id.ToString() }).ToList(),
                ImportTypes = importTypes.Select(x => new SelectListItem() { Text = EnumHelper.GetEnumDescription(x), Value = EnumHelper.GetEnumDescription(x) }).ToList(),
                LastMovementRegistered = lastMovement?.DateExpenditure,
                PaymentMethods = new List<SelectListItem>(),
                MovementPropertyDefinitions = new List<MovementPropertyDefinition>()
                {
                    new MovementPropertyDefinition() { PropertyName = "Date", HasConfig = false },
                    new MovementPropertyDefinition() { PropertyName = "Description", HasConfig = false },
                    new MovementPropertyDefinition() { PropertyName = "Cost", HasConfig = false },
                    new MovementPropertyDefinition() { PropertyName = "Payment Method", HasConfig = true }
                }
            };

            model.PaymentMethods.Add(new SelectListItem() { Text = "Not Applicable" });
            model.PaymentMethods.AddRange(paymentMethods.Where(x => x.Name != "Cash").Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImportMovements(ImportMovementEditModel model)
        {
            model.Expenses.ForEach(x => { x.AccountId = model.AccountId; });
            model.Incomes.ForEach(x => { x.AccountId = model.AccountId; });
            model.AtmWithdraws.ForEach(x => { x.AccountId = model.AccountId; });

            _expenditureService.CreateExpenditures(model.Expenses.ToList());
            _incomeService.CreateIncomes(model.Incomes.ToList());
            _atmWithdrawService.CreateAtmWithdraws(model.AtmWithdraws.ToList());

            return RedirectToAction("Index");
        }
    }
}