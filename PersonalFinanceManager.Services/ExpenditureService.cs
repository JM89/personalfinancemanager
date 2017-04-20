using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using PersonalFinanceManager.Entities;
using AutoMapper;
using PersonalFinanceManager.Models.Expenditure;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Services.MovementStrategy;
using System;
using PersonalFinanceManager.Models.Account;
using PersonalFinanceManager.Models.Dashboard;
using PersonalFinanceManager.Utils.Helpers;
using PersonalFinanceManager.Models.BudgetPlan;
using PersonalFinanceManager.Utils.Helpers;

namespace PersonalFinanceManager.Services
{
    public class ExpenditureService : IExpenditureService
    {
        private readonly IExpenditureRepository _expenditureRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IAtmWithdrawRepository _atmWithdrawRepository;
        private readonly ISavingRepository _savingRepository;
        private readonly IIncomeRepository _incomeRepository;
        private readonly IHistoricMovementRepository _historicMovementRepository;
        private readonly IExpenditureTypeRepository _expenditureTypeRepository;

        public ExpenditureService(IExpenditureRepository expenditureRepository, IBankAccountRepository bankAccountRepository, IAtmWithdrawRepository atmWithdrawRepository, IIncomeRepository incomeRepository,
            IHistoricMovementRepository historicMovementRepository, IExpenditureTypeRepository expenditureTypeRepository, ISavingRepository savingRepository)
        {
            this._expenditureRepository = expenditureRepository;
            this._bankAccountRepository = bankAccountRepository;
            this._atmWithdrawRepository = atmWithdrawRepository;
            this._incomeRepository = incomeRepository;
            this._historicMovementRepository = historicMovementRepository;
            this._expenditureTypeRepository = expenditureTypeRepository;
            this._savingRepository = savingRepository;
        }

        public void CreateExpenditure(ExpenditureEditModel expenditureEditModel)
        {
            var expenditureModel = Mapper.Map<ExpenditureModel>(expenditureEditModel);

            var movement = new Movement(expenditureEditModel);

            var strategy = ContextMovementStrategy.GetMovementStrategy(movement, _bankAccountRepository, _historicMovementRepository, _incomeRepository, _atmWithdrawRepository);
            strategy.Debit();

            if (movement.TargetIncomeId.HasValue)
                expenditureModel.GeneratedIncomeId = movement.TargetIncomeId.Value;

            _expenditureRepository.Create(expenditureModel);
        }
        
        public void EditExpenditure(ExpenditureEditModel expenditureEditModel)
        {
            var expenditureModel = _expenditureRepository.GetById(expenditureEditModel.Id, true);

            var oldMovement = new Movement(Mapper.Map<ExpenditureEditModel>(expenditureModel));

            expenditureModel = Mapper.Map<ExpenditureModel>(expenditureEditModel);
            if (expenditureModel.GeneratedIncomeId.HasValue)
            {
                expenditureModel.GeneratedIncomeId = (int?)null;
                _expenditureRepository.Update(expenditureModel);
            }

            var strategy = ContextMovementStrategy.GetMovementStrategy(oldMovement, _bankAccountRepository, _historicMovementRepository, _incomeRepository, _atmWithdrawRepository);
            var newMovement = new Movement(expenditureEditModel);

            strategy.UpdateDebit(newMovement);

            if (newMovement.TargetIncomeId.HasValue)
            {
                // Update the GenerateIncomeId.
                expenditureModel.GeneratedIncomeId = newMovement.TargetIncomeId.Value;
            }

            _expenditureRepository.Update(expenditureModel);
        }

        public void DeleteExpenditure(int id)
        {
            var expenditureModel = _expenditureRepository.GetById(id);
            var expenditureEditModel = Mapper.Map<ExpenditureEditModel>(expenditureModel);

            _expenditureRepository.Delete(expenditureModel);

            var strategy = ContextMovementStrategy.GetMovementStrategy(new Movement(expenditureEditModel), _bankAccountRepository, _historicMovementRepository, _incomeRepository, _atmWithdrawRepository);
            strategy.Credit();
        }

        public ExpenditureEditModel GetById(int id)
        {
            var expenditure = _expenditureRepository.GetList()
                                    .Include(u => u.Account.Currency)
                                    .Include(u => u.TypeExpenditure)
                                    .Include(u => u.PaymentMethod).SingleOrDefault(x => x.Id == id);

            if (expenditure == null)
            {
                return null;
            }

            return Mapper.Map<ExpenditureEditModel>(expenditure);
        }

        public void ChangeDebitStatus(int id, bool debitStatus)
        {
            var expenditure = _expenditureRepository.GetById(id);
            expenditure.HasBeenAlreadyDebited = debitStatus;
            _expenditureRepository.Update(expenditure);
        }

        public IList<ExpenditureListModel> GetExpenditures(Models.SearchParameters.ExpenditureGetListSearchParameters search)
        {
            var searchParameters = Mapper.Map<Entities.SearchParameters.ExpenditureGetListSearchParameters>(search);
            var expenditures = _expenditureRepository.GetByParameters(searchParameters).ToList();

            if (!string.IsNullOrEmpty(search.UserId))
            {
                var accounts = _bankAccountRepository.GetList().Where(x => x.User_Id == search.UserId).Select(x => x.Id);
                expenditures = expenditures.Where(x => accounts.Contains(x.AccountId)).ToList();
            }

            var mappedExpenditures = expenditures.Select(Mapper.Map<ExpenditureListModel>);
            return mappedExpenditures.ToList();
        }

        public ExpenseSummaryModel GetExpenseSummary(int accountId, BudgetPlanEditModel budgetPlan)
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

            var categories = _expenditureTypeRepository.GetList2().GroupBy(x => x.Id).ToDictionary(x => x.Key, y => y.Single());

            var over12MonthsNames = over12MonthsInterval.GetIntervalsByMonth();
            var currentMonthName = currentMonthInterval.GetSingleMonthName();

            // Retrieve both current month expenses and over 12 months expenses
            var expenses = GetExpenditures(new Models.SearchParameters.ExpenditureGetListSearchParameters
            {
                AccountId = accountId,
                StartDate = over12MonthsInterval.StartDate,
                EndDate = currentMonthInterval.EndDate
            });

            var incomes = _incomeRepository.GetList().Where(x => x.AccountId == accountId).ToList().Where(x =>
                x.DateIncome >= over12MonthsInterval.StartDate && x.DateIncome < currentMonthInterval.EndDate).ToList();

            var savings = _savingRepository.GetList().Where(x => x.AccountId == accountId).ToList().Where(x =>
                x.DateSaving >= over12MonthsInterval.StartDate && x.DateSaving < currentMonthInterval.EndDate).ToList(); 

            // Reset the start date to the first movement (First day of the same month)
            over12MonthsInterval.StartDate = DateTimeFormatHelper.GetFirstDayOfMonth(
                expenses.Any() ?
                expenses.OrderBy(x => x.DateExpenditure).First().DateExpenditure :
                today);

            // Count the number of months in the interval
            var nbMonthInterval = over12MonthsInterval.Count(DateTimeUnitEnums.Months);
            if (nbMonthInterval == 0)
                nbMonthInterval = 1; // No expenses -> no division by zero

            // Get current budget plan if it exists
            var budgetPlanByCategory =
                budgetPlan?.ExpenditureTypes.GroupBy(x => x.ExpenditureType.Id).ToDictionary(x => x.Key, y => y.Single().ExpectedValue)
                ?? categories.ToDictionary(x => x.Key, y => (decimal)0.00);

            var expensesByCategories = expenses.Any()
                ? expenses.GroupBy(x => x.TypeExpenditureId).ToDictionary(x => x.Key, y => y.ToList())
                : categories.ToDictionary(x => x.Key, y => new List<ExpenditureListModel>());

            var expensesByCategoryModel = new List<ExpenseSummaryByCategoryModel>();
            foreach (var exp in expensesByCategories)
            {
                var category = categories[exp.Key];
                var expensesCurrentMonth = exp.Value.Where(x => currentMonthInterval.IsBetween(x.DateExpenditure)).ToList();
                var expensesPreviousMonth = exp.Value.Where(x => previousInterval.IsBetween(x.DateExpenditure)).ToList();
                var expensesOver12Months = exp.Value.Where(x => over12MonthsInterval.IsBetween(x.DateExpenditure)).ToList();

                // ReSharper disable once UseObjectOrCollectionInitializer
                var model = new ExpenseSummaryByCategoryModel();
                model.CurrencySymbol = account.Currency.Symbol;
                model.CategoryId = category.Id;
                model.CategoryName = category.Name;
                model.CategoryColor = category.GraphColor;
                model.CostCurrentMonth = expensesCurrentMonth.Sum(x => x.Cost);
                model.CostPreviousMonth = expensesPreviousMonth.Sum(x => x.Cost);
                model.CostPlannedMonthly = budgetPlanByCategory.ContainsKey(exp.Key) ? budgetPlanByCategory[exp.Key] : 0;
                model.CostOver12Month = expensesOver12Months.Sum(x => x.Cost);
                model.AverageCostOver12Months = model.CostOver12Month / nbMonthInterval;

                // Retrieve the expenses per months (details and summary)
                foreach (var month in over12MonthsNames)
                {
                    var interval = month.Value;
                    var expByMonth = exp.Value.Where(x => interval.IsBetween(x.DateExpenditure)).ToList();

                    model.Expenses.Add(month.Key, expByMonth);
                    model.ExpensesByMonth.Add(month.Key, new ExpenseSummaryByCategoryAndByMonthModel(expByMonth.Sum(x => x.Cost)));
                }
                model.Expenses.Add(currentMonthName, expensesCurrentMonth);
                model.ExpensesByMonth.Add(currentMonthName, new ExpenseSummaryByCategoryAndByMonthModel(expensesCurrentMonth.Sum(x => x.Cost)));

                expensesByCategoryModel.Add(model);
            }

            var totalExpensesOver12Months = expenses.Where(x => over12MonthsInterval.IsBetween(x.DateExpenditure)).Sum(x => x.Cost);

            // Get actual/expected expenses by month for last 12 Months 
            var budgetPlanExpenses = budgetPlanByCategory.Values.Sum(x => x);
            var detailedExpensesOver12Months = new Dictionary<string, ExpenseSummaryByMonthModel>();
            foreach (var month in over12MonthsNames)
            {
                var interval = month.Value;
                var expByMonth = expenses.Where(x => interval.IsBetween(x.DateExpenditure)).Sum(x => x.Cost);
                detailedExpensesOver12Months.Add(month.Key, new ExpenseSummaryByMonthModel()
                {
                    ExpenseValue = expByMonth,
                    ExpenseExpectedValue = budgetPlanExpenses
                });
            }
            detailedExpensesOver12Months.Add(currentMonthName, new ExpenseSummaryByMonthModel()
            {
                ExpenseValue = expenses.Where(x => currentMonthInterval.IsBetween(x.DateExpenditure)).Sum(x => x.Cost),
                ExpenseExpectedValue = budgetPlanExpenses
            });

            // Get the incomes/expenses/savings by month for last 6 months
            var over6MonthsNames = over6MonthsInterval.GetIntervalsByMonth();
            var detailedMovementsOver6Months = new Dictionary<string, ExpenseSummaryByMonthModel>();
            foreach (var month in over6MonthsNames)
            {
                var interval = month.Value;
                var incomeByMonth = incomes.Where(x => interval.IsBetween(x.DateIncome)).Sum(x => x.Cost);
                var savingByMonth = savings.Where(x => interval.IsBetween(x.DateSaving)).Sum(x => x.Amount);
                detailedMovementsOver6Months.Add(month.Key, new ExpenseSummaryByMonthModel()
                {
                    ExpenseValue = detailedExpensesOver12Months[month.Key].ExpenseValue,
                    IncomeValue = incomeByMonth, 
                    SavingValue = savingByMonth
                });
            }
            detailedMovementsOver6Months.Add(currentMonthName, new ExpenseSummaryByMonthModel()
            {
                ExpenseValue = detailedExpensesOver12Months[currentMonthName].ExpenseValue,
                IncomeValue = incomes.Where(x => currentMonthInterval.IsBetween(x.DateIncome)).Sum(x => x.Cost),
                SavingValue = savings.Where(x => currentMonthInterval.IsBetween(x.DateSaving)).Sum(x => x.Amount)
            });

            var expenditureSummaryModel = new ExpenseSummaryModel()
            {
                Account = Mapper.Map<AccountEditModel>(account),
                ExpensesByCategory = expensesByCategoryModel.OrderByDescending(x => x.CostCurrentMonth).ToList(),
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
                CurrentMonthTotalExpense = expenses.Where(x => currentMonthInterval.IsBetween(x.DateExpenditure)).Sum(x => x.Cost),
                AverageExpenses = expenses.Where(x => over12MonthsInterval.IsBetween(x.DateExpenditure)).Sum(x => x.Cost) / nbMonthInterval,
                AverageIncomes = incomes.Where(x =>over12MonthsInterval.IsBetween(x.DateIncome)).Sum(x => x.Cost) / nbMonthInterval,
                AverageSavings = savings.Where(x => over12MonthsInterval.IsBetween(x.DateSaving)).Sum(x => x.Amount) / nbMonthInterval
            };

            return expenditureSummaryModel;
        }
    }
}