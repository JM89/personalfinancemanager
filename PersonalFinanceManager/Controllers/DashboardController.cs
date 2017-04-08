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
    public class DashboardController : BaseController
    {
        private readonly IExpenditureService _expenditureService;
        private readonly IBudgetPlanService _budgetPlanService;

        public DashboardController(IExpenditureService expenditureService, IBankAccountService bankAccountService, IBudgetPlanService budgetPlanService) : base(bankAccountService)
        {
            this._budgetPlanService = budgetPlanService;
            this._expenditureService = expenditureService;
        }

        public ActionResult SplitByTypeDashboard()
        {
            // Get current budget plan if it exists
            var budgetPlan = _budgetPlanService.GetCurrent(CurrentAccount);

            // Get the expense summary by category
            var expenditureSummaryModel = _expenditureService.GetExpenseSummaryByCategory(CurrentAccount, budgetPlan);

            return View("MovementDashboard", expenditureSummaryModel);
        }
    }
}