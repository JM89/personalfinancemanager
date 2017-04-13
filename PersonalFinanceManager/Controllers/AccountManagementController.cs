using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PersonalFinanceManager.Models.Expenditure;
using PersonalFinanceManager.Helpers;
using PersonalFinanceManager.Models.BudgetPlan;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.Services.RequestObjects;
using PersonalFinanceManager.Models.Dashboard;

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
            // Get current budget plan if it exists
            var budgetPlan = _budgetPlanService.GetCurrent(CurrentAccount);

            // Get the expense summary by category
            var expenditureSummaryModel = _expenditureService.GetExpenseSummary(CurrentAccount, budgetPlan);

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
    }
}