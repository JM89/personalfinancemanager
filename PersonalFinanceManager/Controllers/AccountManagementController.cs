using System.Web.Mvc;
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Controllers
{
    public class AccountManagementController : BaseController
    {
        private readonly IExpenditureService _expenditureService;
        private readonly IBudgetPlanService _budgetPlanService;

        public AccountManagementController(IExpenditureService expenditureService, IBankAccountService bankAccountService, IBudgetPlanService budgetPlanService) : base(bankAccountService)
        {
            this._budgetPlanService = budgetPlanService;
            this._expenditureService = expenditureService;
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

        public ActionResult ImportMovements()
        {
            return View();
        }
    }
}