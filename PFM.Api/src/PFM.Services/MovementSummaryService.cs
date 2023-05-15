﻿using AutoMapper;
using PFM.Api.Contracts.BudgetPlan;
using PFM.Api.Contracts.Dashboard;
using PFM.Api.Contracts.Expense;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.SearchParameters;
using PFM.Services.Caches.Interfaces;
using PFM.Services.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFM.Services.Interfaces.Services
{
    public class MovementSummaryService : IMovementSummaryService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly ISavingRepository _savingRepository;
        private readonly IIncomeRepository _incomeRepository;
        private readonly IExpenseTypeRepository _expenseTypeRepository;
        private readonly IBankAccountCache _bankAccountCache;

        public MovementSummaryService(IExpenseRepository ExpenseRepository, IIncomeRepository incomeRepository,
            IExpenseTypeRepository ExpenseTypeRepository, ISavingRepository savingRepository, IBankAccountCache bankAccountCache)
        {
            this._expenseRepository = ExpenseRepository;
            this._incomeRepository = incomeRepository;
            this._expenseTypeRepository = ExpenseTypeRepository;
            this._savingRepository = savingRepository;
            this._bankAccountCache = bankAccountCache;
        }

        public async Task<ExpenseSummary> GetExpenseSummary(int accountId, BudgetPlanDetails budgetPlan, DateTime referenceDate)
        {
            var account = await _bankAccountCache.GetById(accountId);

            if (account == null)
            {
                throw new ArgumentException("Account can't be null.");
            }

            if (budgetPlan?.Id == 0)
            {
                budgetPlan = null;
            }

            var over12MonthsInterval = new Interval(referenceDate, DateTimeUnitEnums.Years, 1);
            var over6MonthsInterval = new Interval(referenceDate, DateTimeUnitEnums.Months, 6);
            var currentMonthInterval = new Interval(referenceDate, referenceDate);
            var previousInterval = new Interval(referenceDate, DateTimeUnitEnums.Months, 1);

            var categories = _expenseTypeRepository.GetList2().GroupBy(x => x.Id).ToDictionary(x => x.Key, y => y.Single());

            var over12MonthsNames = over12MonthsInterval.GetIntervalsByMonth();
            var currentMonthName = currentMonthInterval.GetSingleMonthName();

            // Retrieve both current month expenses and over 12 months expenses

            var expenses = _expenseRepository.GetByParameters(new ExpenseGetListSearchParameters
            {
                AccountId = accountId,
                StartDate = over12MonthsInterval.StartDate,
                EndDate = currentMonthInterval.EndDate
            }).Select(Mapper.Map<ExpenseList>);

            // Reset the start date to the first movement (First day of the same month)
            over12MonthsInterval.StartDate = DateTimeFormatHelper.GetFirstDayOfMonth(
                expenses.Any() ?
                expenses.OrderBy(x => x.DateExpense).First().DateExpense :
                referenceDate);

            // Count the number of months in the interval
            var nbMonthInterval = over12MonthsInterval.Count(DateTimeUnitEnums.Months);
            if (nbMonthInterval == 0)
                nbMonthInterval = 1; // No expenses -> no division by zero

            // Get current budget plan if it exists
            var budgetPlanByCategory =
                budgetPlan?.ExpenseTypes.GroupBy(x => x.ExpenseType.Id).ToDictionary(x => x.Key, y => y.Single().ExpectedValue)
                ?? categories.ToDictionary(x => x.Key, y => (decimal)0.00);

            var expensesByCategories = expenses.Any()
                ? expenses.GroupBy(x => x.ExpenseTypeId).ToDictionary(x => x.Key, y => y.ToList())
                : categories.ToDictionary(x => x.Key, y => new List<ExpenseList>());

            var expensesByCategory = new List<ExpenseSummaryByCategory>();
            foreach (var exp in expensesByCategories)
            {
                var category = categories[exp.Key];
                var expensesCurrentMonth = exp.Value.Where(x => currentMonthInterval.IsBetween(x.DateExpense)).ToList();
                var expensesPreviousMonth = exp.Value.Where(x => previousInterval.IsBetween(x.DateExpense)).ToList();
                var expensesOver12Months = exp.Value.Where(x => over12MonthsInterval.IsBetween(x.DateExpense)).ToList();

                // ReSharper disable once UseObjectOrCollectionInitializer
                var expenseSummary = new ExpenseSummaryByCategory();
                expenseSummary.CurrencySymbol = account.CurrencySymbol;
                expenseSummary.CategoryId = category.Id;
                expenseSummary.CategoryName = category.Name;
                expenseSummary.CategoryColor = category.GraphColor;
                expenseSummary.CostCurrentMonth = expensesCurrentMonth.Sum(x => x.Cost);
                expenseSummary.CostPreviousMonth = expensesPreviousMonth.Sum(x => x.Cost);
                expenseSummary.CostPlannedMonthly = budgetPlanByCategory.ContainsKey(exp.Key) ? budgetPlanByCategory[exp.Key] : 0;
                expenseSummary.CostOver12Month = expensesOver12Months.Sum(x => x.Cost);
                expenseSummary.AverageCostOver12Months = expenseSummary.CostOver12Month / nbMonthInterval;

                // Retrieve the expenses per months (details and summary)
                foreach (var month in over12MonthsNames)
                {
                    var interval = month.Value;
                    var expByMonth = exp.Value.Where(x => interval.IsBetween(x.DateExpense)).ToList();

                    expenseSummary.Expenses.Add(month.Key, expByMonth);
                    expenseSummary.ExpensesByMonth.Add(month.Key, new ExpenseSummaryByCategoryAndByMonth(expByMonth.Sum(x => x.Cost)));
                }
                expenseSummary.Expenses.Add(currentMonthName, expensesCurrentMonth);
                expenseSummary.ExpensesByMonth.Add(currentMonthName, new ExpenseSummaryByCategoryAndByMonth(expensesCurrentMonth.Sum(x => x.Cost)));

                expensesByCategory.Add(expenseSummary);
            }

            var totalExpensesOver12Months = expenses.Where(x => over12MonthsInterval.IsBetween(x.DateExpense)).Sum(x => x.Cost);

            // Get actual/expected expenses by month for last 12 Months 
            var budgetPlanExpenses = budgetPlanByCategory.Values.Sum(x => x);
            var detailedExpensesOver12Months = new Dictionary<string, ExpenseSummaryByMonth>();
            foreach (var month in over12MonthsNames)
            {
                var interval = month.Value;
                var expByMonth = expenses.Where(x => interval.IsBetween(x.DateExpense)).Sum(x => x.Cost);
                detailedExpensesOver12Months.Add(month.Key, new ExpenseSummaryByMonth()
                {
                    ExpenseValue = expByMonth,
                    ExpenseExpectedValue = budgetPlanExpenses
                });
            }
            detailedExpensesOver12Months.Add(currentMonthName, new ExpenseSummaryByMonth()
            {
                ExpenseValue = expenses.Where(x => currentMonthInterval.IsBetween(x.DateExpense)).Sum(x => x.Cost),
                ExpenseExpectedValue = budgetPlanExpenses
            });

            var incomes = _incomeRepository.GetList2().Where(x => x.AccountId == accountId && x.DateIncome >= over12MonthsInterval.StartDate && x.DateIncome < currentMonthInterval.EndDate).ToList();
            var savings = _savingRepository.GetList2().Where(x => x.AccountId == accountId && x.DateSaving >= over12MonthsInterval.StartDate && x.DateSaving < currentMonthInterval.EndDate).ToList();

            // Get the incomes/expenses/savings by month for last 6 months
            var over6MonthsNames = over6MonthsInterval.GetIntervalsByMonth();
            var detailedMovementsOver6Months = new Dictionary<string, ExpenseSummaryByMonth>();
            foreach (var month in over6MonthsNames)
            {
                var interval = month.Value;
                var incomeByMonth = incomes.Where(x => interval.IsBetween(x.DateIncome)).Sum(x => x.Cost);
                var savingByMonth = savings.Where(x => interval.IsBetween(x.DateSaving)).Sum(x => x.Amount);
                detailedMovementsOver6Months.Add(month.Key, new ExpenseSummaryByMonth()
                {
                    ExpenseValue = detailedExpensesOver12Months[month.Key].ExpenseValue,
                    IncomeValue = incomeByMonth, 
                    SavingValue = savingByMonth
                });
            }
            detailedMovementsOver6Months.Add(currentMonthName, new ExpenseSummaryByMonth()
            {
                ExpenseValue = detailedExpensesOver12Months[currentMonthName].ExpenseValue,
                IncomeValue = incomes.Where(x => currentMonthInterval.IsBetween(x.DateIncome)).Sum(x => x.Cost),
                SavingValue = savings.Where(x => currentMonthInterval.IsBetween(x.DateSaving)).Sum(x => x.Amount)
            });

            var accountDetails = await _bankAccountCache.GetById(account.Id);

            var ExpenseSummary = new ExpenseSummary()
            {
                Account = accountDetails,
                ExpensesByCategory = expensesByCategory.OrderByDescending(x => x.CostCurrentMonth).ToList(),
                LabelCurrentMonth = DateTimeFormatHelper.GetMonthNameAndYear(referenceDate),
                LabelPreviousMonth = DateTimeFormatHelper.GetMonthNameAndYear(referenceDate.AddMonths(-1)),
                BudgetPlanName = budgetPlan != null ? budgetPlan.Name : string.Empty,
                AccountName = account.Name,
                DisplayDashboard = true,
                CurrencySymbol = account.CurrencySymbol,
                HasCurrentBudgetPlan = budgetPlan != null,
                HasExpenses = expenses.Any(),
                HasCategories = categories.Any(),
                TotalExpensesOver12Months = totalExpensesOver12Months, 
                DetailedExpensesOver12Months = detailedExpensesOver12Months,
                DetailedMovementsOver6Months = detailedMovementsOver6Months,
                CurrentMonthTotalExpense = expenses.Where(x => currentMonthInterval.IsBetween(x.DateExpense)).Sum(x => x.Cost),
                AverageExpenses = expenses.Where(x => over12MonthsInterval.IsBetween(x.DateExpense)).Sum(x => x.Cost) / nbMonthInterval,
                AverageIncomes = incomes.Where(x =>over12MonthsInterval.IsBetween(x.DateIncome)).Sum(x => x.Cost) / nbMonthInterval,
                AverageSavings = savings.Where(x => over12MonthsInterval.IsBetween(x.DateSaving)).Sum(x => x.Amount) / nbMonthInterval
            };

            return ExpenseSummary;
        }
    }
}