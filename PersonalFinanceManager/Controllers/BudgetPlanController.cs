using PersonalFinanceManager.Models.BudgetPlan;
using PersonalFinanceManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalFinanceManager.Controllers
{
    public class BudgetPlanController : BaseController
    {
        private ExpenditureTypeService expenditureTypeService = new ExpenditureTypeService();
        private IncomeService incomeService = new IncomeService();
        private PeriodicOutcomeService periodicOutcomeService = new PeriodicOutcomeService();

        public ActionResult Index()
        {
            var accountId = CurrentAccount;

            var expenditureTypes = expenditureTypeService.GetExpenditureTypes();

            var incomes = incomeService.GetIncomes(accountId);

            var budgetPlanModel = new BudgetPlanEditModel();
            budgetPlanModel.BudgetName = "Monthly Budget";
            budgetPlanModel.Incomes = incomes;
            budgetPlanModel.ExpenditureTypes = new List<BudgetPlanExpenditureType>();

            var periodicOutcomes = periodicOutcomeService.GetPeriodicOutcomes(accountId);

            foreach (var expenditureType in expenditureTypes)
            {
                var budgetPlanExpenditureType = new BudgetPlanExpenditureType();
                budgetPlanExpenditureType.ExpenditureType = expenditureType;
                budgetPlanExpenditureType.PreviousMonthValue = 0;
                budgetPlanExpenditureType.AverageMonthValue = 0;
                budgetPlanExpenditureType.PeriodicOutcomeValue = periodicOutcomes.Where(x => x.TypeExpenditureId == expenditureType.Id && x.FrequencyName == "Monthly").Select(x => x.Cost).Sum();
                budgetPlanModel.ExpenditureTypes.Add(budgetPlanExpenditureType);
            }

            return View("Create", budgetPlanModel);
        }
    }
}