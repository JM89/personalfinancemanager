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
        private readonly IExpenditureTypeService _expenditureTypeService;
        private readonly IBankAccountService _bankAccountService;
        private readonly IBudgetPlanService _budgetPlanService;

        public DashboardController(IExpenditureService expenditureService, IExpenditureTypeService expenditureTypeService,
            IBankAccountService bankAccountService, IBudgetPlanService budgetPlanService) : base(bankAccountService)
        {
            this._budgetPlanService = budgetPlanService;
            this._expenditureTypeService = expenditureTypeService;
            this._expenditureService = expenditureService;
            this._bankAccountService = bankAccountService;
        }

        public ActionResult SplitByTypeDashboard()
        {
            var account = _bankAccountService.GetById(CurrentAccount);
            var categories = _expenditureTypeService.GetExpenditureTypes().ToList().GroupBy(x => x.Id).ToDictionary(x => x.Key, y => y.Single());

            var today = DateTime.Now;
            var over12MonthsInterval = new Interval(today, DateTimeUnitEnums.Years, 1);
            var currentMonthInterval = new Interval(today, today);
            var previousInterval = new Interval(today, DateTimeUnitEnums.Months, 1);

            var over12MonthsNames = over12MonthsInterval.GetIntervalsByMonth();
            var currentMonthName = currentMonthInterval.GetSingleMonthName();

            // Retrieve both current month expenses and over 12 months expenses
            var expenses = _expenditureService.GetExpenditures(new ExpenditureSearch() {
                AccountId = CurrentAccount,
                ShowOnDashboard = true,
                StartDate = over12MonthsInterval.StartDate,
                EndDate = currentMonthInterval.EndDate
            });

            // Reset the start date to the first movement (First day of the same month)
            over12MonthsInterval.StartDate = DateTimeHelper.GetFirstDayOfMonth(expenses.OrderBy(x => x.DateExpenditure).First().DateExpenditure);

            // Get Total Expenses By Month Over 12 Months + Current Month
            var totalExpenses = new Dictionary<string, decimal>();
            foreach (var month in over12MonthsNames)
            {
                var interval = month.Value;
                var expByMonth = expenses.Where(x => interval.IsBetween(x.DateExpenditure)).Sum(x => x.Cost);
                totalExpenses.Add(month.Key, expByMonth);
            }
            totalExpenses.Add(currentMonthName, expenses.Where(x => currentMonthInterval.IsBetween(x.DateExpenditure)).Sum(x => x.Cost));

            // Count the number of months in the interval
            var nbMonthInterval = over12MonthsInterval.Count(DateTimeUnitEnums.Months);

            // Get current budget plan if it exists
            var budgetPlan = _budgetPlanService.GetCurrent(CurrentAccount);
            var budgetPlanByCategory = 
                budgetPlan?.ExpenditureTypes.GroupBy(x => x.ExpenditureType.Id).ToDictionary(x => x.Key, y => y.Single().ExpectedValue) 
                ?? categories.ToDictionary(x => x.Key, y => (decimal)0.00);

            var expensesByCategories = expenses.GroupBy(x => x.TypeExpenditureId);           
            var expensesByCategoryModel = new List<ExpenseSummaryByCategoryModel>();
            foreach (var exp in expensesByCategories)
            {
                var category = categories[exp.Key];
                var expensesCurrentMonth = exp.Where(x => currentMonthInterval.IsBetween(x.DateExpenditure)).ToList();
                var expensesPreviousMonth = exp.Where(x => previousInterval.IsBetween(x.DateExpenditure)).ToList();
                var expensesOver12Months = exp.Where(x => over12MonthsInterval.IsBetween(x.DateExpenditure)).ToList();

                // ReSharper disable once UseObjectOrCollectionInitializer
                var model = new ExpenseSummaryByCategoryModel();
                model.CurrencySymbol = account.CurrencySymbol;
                model.CategoryId = category.Id;
                model.CategoryName = category.Name;
                model.CategoryColor = category.GraphColor;
                model.CostCurrentMonth = expensesCurrentMonth.Sum(x => x.Cost);
                model.CostPreviousMonth = expensesPreviousMonth.Sum(x => x.Cost);
                model.CostPlannedMonthly = budgetPlanByCategory[exp.Key];
                model.CostOver12Month = expensesOver12Months.Sum(x => x.Cost);
                model.AverageCostOver12Months = model.CostOver12Month / nbMonthInterval;

                // Retrieve the expenses per months (details and summary)
                foreach (var month in over12MonthsNames)
                {
                    var interval = month.Value;
                    var expByMonth = exp.Where(x => interval.IsBetween(x.DateExpenditure)).ToList();

                    model.Expenses.Add(month.Key, expByMonth);
                    model.ExpensesByMonth.Add(month.Key, new ExpenseSummaryByCategoryAndByMonthModel(expByMonth.Sum(x => x.Cost), totalExpenses[month.Key]));
                }
                model.Expenses.Add(currentMonthName, expensesCurrentMonth);
                model.ExpensesByMonth.Add(currentMonthName, new ExpenseSummaryByCategoryAndByMonthModel(expensesCurrentMonth.Sum(x => x.Cost), totalExpenses[currentMonthName]));

                expensesByCategoryModel.Add(model);
            }

            var totalExpensesOver12Months = expenses.Where(x => over12MonthsInterval.IsBetween(x.DateExpenditure)).Sum(x => x.Cost);

            var expenditureSummaryModel = new ExpenseSummaryModel()
            {
                ExpensesByCategory = expensesByCategoryModel.OrderByDescending(x => x.CostCurrentMonth).ToList(), 
                LabelCurrentMonth = DateTimeHelper.GetMonthNameAndYear(today),
                LabelPreviousMonth = DateTimeHelper.GetMonthNameAndYear(today.AddMonths(-1)), 
                BudgetPlanName = budgetPlan != null ? budgetPlan.Name : string.Empty,
                AccountName = account.Name,
                DisplayDashboard = true,
                CurrencySymbol = account.CurrencySymbol,
                HasBudget = budgetPlan != null,
                TotalExpensesOver12Months = totalExpensesOver12Months
            };

            return View("SplitByTypeDashboard", expenditureSummaryModel);
        }
    }
}