using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PFM.Services.MovementStrategy;
using System;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DTOs.Expense;
using PFM.DataAccessLayer.Entities;
using PFM.DTOs.BudgetPlan;
using PFM.Utils.Helpers;
using PFM.DTOs.Account;
using PFM.DTOs.Dashboard;

namespace PFM.Services.Interfaces.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _ExpenseRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IAtmWithdrawRepository _atmWithdrawRepository;
        private readonly ISavingRepository _savingRepository;
        private readonly IIncomeRepository _incomeRepository;
        private readonly IHistoricMovementRepository _historicMovementRepository;
        private readonly IExpenseTypeRepository _ExpenseTypeRepository;

        public ExpenseService(IExpenseRepository ExpenseRepository, IBankAccountRepository bankAccountRepository, IAtmWithdrawRepository atmWithdrawRepository, IIncomeRepository incomeRepository,
            IHistoricMovementRepository historicMovementRepository, IExpenseTypeRepository ExpenseTypeRepository, ISavingRepository savingRepository)
        {
            this._ExpenseRepository = ExpenseRepository;
            this._bankAccountRepository = bankAccountRepository;
            this._atmWithdrawRepository = atmWithdrawRepository;
            this._incomeRepository = incomeRepository;
            this._historicMovementRepository = historicMovementRepository;
            this._ExpenseTypeRepository = ExpenseTypeRepository;
            this._savingRepository = savingRepository;
        }

        public void CreateExpenses(List<ExpenseDetails> ExpenseDetails)
        {
            ExpenseDetails.ForEach(CreateExpense);
        }

        public void CreateExpense(ExpenseDetails expenseDetails)
        {
            var Expense = Mapper.Map<Expense>(expenseDetails);

            var movement = new Movement(expenseDetails);

            var strategy = ContextMovementStrategy.GetMovementStrategy(movement, _bankAccountRepository, _historicMovementRepository, _incomeRepository, _atmWithdrawRepository);
            strategy.Debit();

            if (movement.TargetIncomeId.HasValue)
                Expense.GeneratedIncomeId = movement.TargetIncomeId.Value;

            _ExpenseRepository.Create(Expense);
        }
        
        public void EditExpense(ExpenseDetails ExpenseDetails)
        {
            var Expense = _ExpenseRepository.GetById(ExpenseDetails.Id, true);

            var oldMovement = new Movement(Mapper.Map<ExpenseDetails>(Expense));

            Expense = Mapper.Map<Expense>(ExpenseDetails);
            if (Expense.GeneratedIncomeId.HasValue)
            {
                Expense.GeneratedIncomeId = (int?)null;
                _ExpenseRepository.Update(Expense);
            }

            var strategy = ContextMovementStrategy.GetMovementStrategy(oldMovement, _bankAccountRepository, _historicMovementRepository, _incomeRepository, _atmWithdrawRepository);
            var newMovement = new Movement(ExpenseDetails);

            strategy.UpdateDebit(newMovement);

            if (newMovement.TargetIncomeId.HasValue)
            {
                // Update the GenerateIncomeId.
                Expense.GeneratedIncomeId = newMovement.TargetIncomeId.Value;
            }

            _ExpenseRepository.Update(Expense);
        }

        public void DeleteExpense(int id)
        {
            var Expense = _ExpenseRepository.GetById(id);
            var ExpenseDetails = Mapper.Map<ExpenseDetails>(Expense);

            _ExpenseRepository.Delete(Expense);

            var strategy = ContextMovementStrategy.GetMovementStrategy(new Movement(ExpenseDetails), _bankAccountRepository, _historicMovementRepository, _incomeRepository, _atmWithdrawRepository);
            strategy.Credit();
        }

        public ExpenseDetails GetById(int id)
        {
            var Expense = _ExpenseRepository
                                    .GetList2(u => u.Account.Currency, u => u.ExpenseType, u => u.PaymentMethod)
                                    .SingleOrDefault(x => x.Id == id);

            if (Expense == null)
            {
                return null;
            }

            return Mapper.Map<ExpenseDetails>(Expense);
        }

        public void ChangeDebitStatus(int id, bool debitStatus)
        {
            var Expense = _ExpenseRepository.GetById(id);
            Expense.HasBeenAlreadyDebited = debitStatus;
            _ExpenseRepository.Update(Expense);
        }

        public IList<ExpenseList> GetExpenses(PFM.DTOs.SearchParameters.ExpenseGetListSearchParameters search)
        {
            var searchParameters = Mapper.Map<PFM.DataAccessLayer.SearchParameters.ExpenseGetListSearchParameters>(search);
            var Expenses = _ExpenseRepository.GetByParameters(searchParameters).ToList();

            if (!string.IsNullOrEmpty(search.UserId))
            {
                var accounts = _bankAccountRepository.GetList().Where(x => x.User_Id == search.UserId).Select(x => x.Id);
                Expenses = Expenses.Where(x => accounts.Contains(x.AccountId)).ToList();
            }

            var mappedExpenses = Expenses.Select(Mapper.Map<ExpenseList>);
            return mappedExpenses.ToList();
        }

        public ExpenseSummary GetExpenseSummary(int accountId, BudgetPlanDetails budgetPlan)
        {
            var account = _bankAccountRepository.GetById(accountId, x => x.Bank, x => x.Currency);

            if (account == null)
            {
                throw new ArgumentException("Account can't be null.");
            }

            var today = DateTime.Now;
            var over12MonthsInterval = new Interval(today, DateTimeUnitEnums.Years, 1);
            var over6MonthsInterval = new Interval(today, DateTimeUnitEnums.Months, 6);
            var currentMonthInterval = new Interval(today, today);
            var previousInterval = new Interval(today, DateTimeUnitEnums.Months, 1);

            var categories = _ExpenseTypeRepository.GetList2().GroupBy(x => x.Id).ToDictionary(x => x.Key, y => y.Single());

            var over12MonthsNames = over12MonthsInterval.GetIntervalsByMonth();
            var currentMonthName = currentMonthInterval.GetSingleMonthName();

            // Retrieve both current month expenses and over 12 months expenses
            var expenses = GetExpenses(new PFM.DTOs.SearchParameters.ExpenseGetListSearchParameters
            {
                AccountId = accountId,
                StartDate = over12MonthsInterval.StartDate,
                EndDate = currentMonthInterval.EndDate
            });

            // Reset the start date to the first movement (First day of the same month)
            over12MonthsInterval.StartDate = DateTimeFormatHelper.GetFirstDayOfMonth(
                expenses.Any() ?
                expenses.OrderBy(x => x.DateExpense).First().DateExpense :
                today);

            // Count the number of months in the interval
            var nbMonthInterval = over12MonthsInterval.Count(DateTimeUnitEnums.Months);
            if (nbMonthInterval == 0)
                nbMonthInterval = 1; // No expenses -> no division by zero

            // Get current budget plan if it exists
            var budgetPlanByCategory =
                budgetPlan?.ExpenseTypes.GroupBy(x => x.ExpenseType.Id).ToDictionary(x => x.Key, y => y.Single().ExpectedValue)
                ?? categories.ToDictionary(x => x.Key, y => (decimal)0.00);

            var expensesByCategories = expenses.Any()
                ? expenses.GroupBy(x => x.TypeExpenseId).ToDictionary(x => x.Key, y => y.ToList())
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
                expenseSummary.CurrencySymbol = account.Currency.Symbol;
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

            var incomes = _incomeRepository.GetList().Where(x => x.AccountId == accountId && x.DateIncome >= over12MonthsInterval.StartDate && x.DateIncome < currentMonthInterval.EndDate).ToList();
            var savings = _savingRepository.GetList().Where(x => x.AccountId == accountId && x.DateSaving >= over12MonthsInterval.StartDate && x.DateSaving < currentMonthInterval.EndDate).ToList();

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

            var ExpenseSummary = new ExpenseSummary()
            {
                Account = Mapper.Map<AccountDetails>(account),
                ExpensesByCategory = expensesByCategory.OrderByDescending(x => x.CostCurrentMonth).ToList(),
                LabelCurrentMonth = DateTimeFormatHelper.GetMonthNameAndYear(today),
                LabelPreviousMonth = DateTimeFormatHelper.GetMonthNameAndYear(today.AddMonths(-1)),
                BudgetPlanName = budgetPlan != null ? budgetPlan.Name : string.Empty,
                AccountName = account.Name,
                DisplayDashboard = true,
                CurrencySymbol = account.Currency.Symbol,
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