using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PFM.Services.Interfaces;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.SearchParameters;
using PFM.Api.Contracts.BudgetPlan;
using PFM.Services.Utils.Helpers;
using PFM.Api.Contracts.ExpenseType;
using PFM.Services.Caches.Interfaces;
using System.Threading.Tasks;

namespace PFM.Services
{
    public class BudgetPlanService : IBudgetPlanService
    {
        private readonly IBudgetPlanRepository _budgetPlanRepository;
        private readonly IBudgetByExpenseTypeRepository _budgetByExpenseTypeRepository;
        private readonly IBankAccountCache _bankAccountCache;
        private readonly IExpenseRepository _ExpenseRepository;
        private readonly IExpenseTypeRepository _ExpenseTypeRepository;
        private readonly IIncomeRepository _incomeRepository;
        private readonly ISavingRepository _savingRepository;

        public BudgetPlanService(IBudgetPlanRepository budgetPlanRepository, IBudgetByExpenseTypeRepository budgetByExpenseTypeRepository, IBankAccountCache bankAccountCache,
            IExpenseRepository ExpenseRepository, IExpenseTypeRepository ExpenseTypeRepository, IIncomeRepository incomeRepository, 
            ISavingRepository savingRepository)
        {
            this._budgetPlanRepository = budgetPlanRepository;
            this._budgetByExpenseTypeRepository = budgetByExpenseTypeRepository;
            this._bankAccountCache = bankAccountCache;
            this._ExpenseRepository = ExpenseRepository;
            this._ExpenseTypeRepository = ExpenseTypeRepository;
            this._incomeRepository = incomeRepository;
            this._savingRepository = savingRepository;
        }

        /// <summary>
        /// Return the list of budget plans.
        /// </summary>
        /// <returns></returns>
        public IList<BudgetPlanList> GetBudgetPlans(int accountId)
        {
            var budgetPlansForAccount = _budgetByExpenseTypeRepository.GetList2().Where(x => x.AccountId == accountId).Select(x => x.BudgetPlanId);

            var budgetPlans = _budgetPlanRepository.GetList().Where(x => budgetPlansForAccount.Contains(x.Id)).ToList();

            return budgetPlans.Select(Mapper.Map<BudgetPlanList>).ToList();
        }
        
        public BudgetPlanDetails GetCurrent(int accountId)
        {
            var budgetPlansForAccount = _budgetByExpenseTypeRepository.GetList2().Where(x => x.AccountId == accountId).Select(x => x.BudgetPlanId);

            var currentBudgetPlan = _budgetPlanRepository.GetList().SingleOrDefault(x => budgetPlansForAccount.Contains(x.Id) && x.StartDate.HasValue && !x.EndDate.HasValue);
            if (currentBudgetPlan != null)
            {
                return GetById(currentBudgetPlan.Id);
            }
            return null;
        }

        public BudgetPlanDetails GetById(int id)
        {
            var budgetPlan = _budgetPlanRepository.GetById(id);
            if (budgetPlan == null)
            {
                return null;
            }

            var budgetPlanExpenses = _budgetByExpenseTypeRepository.GetList2(x => x.ExpenseType).Where(x => x.BudgetPlanId == id);

            var budgetExpenseTypes = new List<BudgetPlanExpenseType>();
            foreach(var budgetPlanExpense in budgetPlanExpenses)
            {
                var expenseType = budgetPlanExpense.ExpenseType;
                var mappedeExpenseType = Mapper.Map<ExpenseTypeList>(expenseType);
                var budgetExpenseType = new BudgetPlanExpenseType()
                {
                    ExpenseType = mappedeExpenseType,
                    ExpectedValue = budgetPlanExpense.Budget
                };
                budgetExpenseTypes.Add(budgetExpenseType);
            }

            var mappedBudgetPlan = Mapper.Map<BudgetPlanDetails>(budgetPlan);
            mappedBudgetPlan.ExpenseTypes = budgetExpenseTypes.ToList();

            return mappedBudgetPlan;
        }

        /// <summary>
        /// Create a budget plan.
        /// </summary>
        /// <param name="budgetPlanDetails"></param>
        /// <param name="accountId"></param>
        public void CreateBudgetPlan(BudgetPlanDetails budgetPlanDetails, int accountId)
        {
            var budgetPlan = Mapper.Map<BudgetPlan>(budgetPlanDetails);
            _budgetPlanRepository.Create(budgetPlan);

            foreach(var ExpenseType in budgetPlanDetails.ExpenseTypes)
            {
                var plannedExpense = new BudgetByExpenseType
                {
                    Budget = ExpenseType.ExpectedValue,
                    ExpenseTypeId = ExpenseType.ExpenseType.Id,
                    BudgetPlanId = budgetPlan.Id,
                    AccountId = accountId
                };

                // Account ID is defined at BudgetByExpenseType time because at first, I wanted to plan budget through several accounts.
                // It might evolve later so for now, I keep it here, independently from BudgetByExpenseType.

                _budgetByExpenseTypeRepository.Create(plannedExpense);
            }
        }

        /// <summary>
        /// Edit a budget plan.
        /// </summary>
        /// <param name="budgetPlanDetails"></param>
        /// <param name="accountId"></param>
        public void EditBudgetPlan(BudgetPlanDetails budgetPlanDetails, int accountId)
        {
            var budgetPlan = _budgetPlanRepository.GetById(budgetPlanDetails.Id);
            budgetPlan.Name = budgetPlanDetails.Name;
            budgetPlan.ExpectedIncomes = budgetPlanDetails.ExpectedIncomes;
            budgetPlan.ExpectedSavings = budgetPlanDetails.ExpectedSavings;
            _budgetPlanRepository.Update(budgetPlan);

            var existingBudgetPlanExpenses = _budgetByExpenseTypeRepository.GetList2(x => x.ExpenseType)
                  .Where(x => x.BudgetPlanId == budgetPlanDetails.Id);

            foreach (var budgetExpenseType in budgetPlanDetails.ExpenseTypes)
            {
                var existingBudgetExpenseType = existingBudgetPlanExpenses.SingleOrDefault(x => x.ExpenseType.Id == budgetExpenseType.ExpenseType.Id);

                if (existingBudgetExpenseType == null)
                {
                    // Add
                    var plannedExpense = new BudgetByExpenseType()
                    {
                        Budget = budgetExpenseType.ExpectedValue,
                        ExpenseTypeId = budgetExpenseType.ExpenseType.Id,
                        BudgetPlanId = budgetPlanDetails.Id,
                        AccountId = accountId
                    };
                    _budgetByExpenseTypeRepository.Create(plannedExpense);
                }
                else
                {
                    // Update
                    existingBudgetExpenseType.Budget = budgetExpenseType.ExpectedValue;
                    _budgetByExpenseTypeRepository.Update(existingBudgetExpenseType);
                }
            }
        }

        public void StartBudgetPlan(int value, int accountId)
        {
            var budgetPlansForAccount = _budgetByExpenseTypeRepository.GetList().Where(x => x.AccountId == accountId).Select(x => x.BudgetPlanId);
            var currentBudgetPlan = _budgetPlanRepository.GetList().SingleOrDefault(x => budgetPlansForAccount.Contains(x.Id) && x.StartDate.HasValue && !x.EndDate.HasValue);
            if (currentBudgetPlan != null)
            {
                currentBudgetPlan.EndDate = DateTime.Now;
                _budgetPlanRepository.Update(currentBudgetPlan);
            }

            var budgetPlan = _budgetPlanRepository.GetById(value);
            var nextMonth = DateTime.Now.AddMonths(1);
            var firstOfNextMonth = new DateTime(nextMonth.Year,nextMonth.Month, 1);
            budgetPlan.StartDate = firstOfNextMonth;

            _budgetPlanRepository.Update(budgetPlan);
        }

        public void StopBudgetPlan(int value)
        {
            var budgetPlan = _budgetPlanRepository.GetById(value);
            budgetPlan.EndDate = DateTime.Now;
            _budgetPlanRepository.Update(budgetPlan);
        }

        public async Task<BudgetPlanDetails> BuildBudgetPlan(int accountId, int? budgetPlanId = null)
        {
            var currencySymbol = (await _bankAccountCache.GetById(accountId)).CurrencySymbol;

            var today = DateTime.Now;
            var over12MonthsInterval = new Interval(today, DateTimeUnitEnums.Years, 1);
            var previousInterval = new Interval(today, DateTimeUnitEnums.Months, 1);
            var firstOfNextMonth = DateTimeFormatHelper.GetFirstDayOfMonth(today.AddMonths(1));

            // Retrieve the categories
            var categories = _ExpenseTypeRepository.GetList2().GroupBy(x => x.Id).ToDictionary(x => x.Key, y => y.Single());

            // Retrieve the expenses over the last 12 months (excluding current month) 
            var expensesOver12Months = _ExpenseRepository.GetByParameters(new ExpenseGetListSearchParameters()
            {
                AccountId = accountId,
                StartDate = over12MonthsInterval.StartDate,
                EndDate = over12MonthsInterval.EndDate
            });

            // Group by category the expenses over the last 12 months
            var expensesOver12MonthsByCategory = expensesOver12Months.GroupBy(x => x.ExpenseTypeId).ToDictionary(x => x.Key, y => y.ToList());

            over12MonthsInterval.StartDate = DateTimeFormatHelper.GetFirstDayOfMonth(
              expensesOver12Months.Any() ?
              expensesOver12Months.OrderBy(x => x.DateExpense).First().DateExpense :
              today);

            var nbMonthInterval = over12MonthsInterval.Count(DateTimeUnitEnums.Months);
            if (nbMonthInterval == 0)
                nbMonthInterval = 1; // No expenses -> no division by zero

            // Retrieve the expenses last months and group by category
            var lastMonthExpenses = expensesOver12Months.Where(x => previousInterval.IsBetween(x.DateExpense)).ToList();
            var lastMonthExpensesByCategory = lastMonthExpenses.GroupBy(x => x.ExpenseTypeId).ToDictionary(x => x.Key, y => y.ToList());

            // Get the current Budget Plan for the account. If none, returns a default of cost of 0.00
            var currentBudgetPlan = GetCurrent(accountId);
            var currentBudgetPlanByCategory = currentBudgetPlan?.ExpenseTypes
                                                    .GroupBy(x => x.ExpenseType.Id)
                                                    .ToDictionary(x => x.Key, y => y.Single().ExpectedValue);

            // Get the existing Budget Plan for the provided ID. If none, returns a default of cost of 0.00
            var existingBudgetPlan = budgetPlanId.HasValue ? GetById(budgetPlanId.Value) : null;
            var existingBudgetPlanByCategory = existingBudgetPlan?.ExpenseTypes.GroupBy(x => x.ExpenseType.Id).ToDictionary(x => x.Key, y => y.Single().ExpectedValue);

            BudgetPlanDetails budgetPlan = null;

            if (existingBudgetPlan != null)
            {
                budgetPlan = new BudgetPlanDetails()
                {
                    Id = existingBudgetPlan.Id,
                    Name = existingBudgetPlan.Name,
                    ExpenseTypes = new List<BudgetPlanExpenseType>(),
                    CurrencySymbol = currencySymbol,
                    StartDate = existingBudgetPlan.StartDate,
                    EndDate = existingBudgetPlan.EndDate,
                    PlannedStartDate = firstOfNextMonth,
                    HasCurrentBudgetPlan = currentBudgetPlan != null,
                    BudgetPlanName = currentBudgetPlan?.Name
                };
            }
            else
            {
                budgetPlan = new BudgetPlanDetails()
                {
                    ExpenseTypes = new List<BudgetPlanExpenseType>(),
                    CurrencySymbol = currencySymbol,
                    HasCurrentBudgetPlan = currentBudgetPlan != null,
                    BudgetPlanName = currentBudgetPlan?.Name
                };
            }

            foreach (var category in categories)
            {
                var expectedValue = 0.00M; var currentBudgetPlanValue = 0.00M;

                if (currentBudgetPlan != null)
                {
                    currentBudgetPlanValue = currentBudgetPlanByCategory.ContainsKey(category.Key)
                                                ? currentBudgetPlanByCategory[category.Key]
                                                : 0.00M;
                }

                if (existingBudgetPlan != null)
                {
                    expectedValue = existingBudgetPlanByCategory.ContainsKey(category.Key)
                                        ? existingBudgetPlanByCategory[category.Key]
                                        : 0.00M;
                }
                else if (currentBudgetPlan != null)
                {
                    expectedValue = currentBudgetPlanValue;
                }

                var previousMonthValue = lastMonthExpensesByCategory.ContainsKey(category.Key)
                                            ? lastMonthExpensesByCategory[category.Key].Sum(x => x.Cost)
                                            : 0.00M;

                var averageMonthValue = expensesOver12MonthsByCategory.ContainsKey(category.Key)
                                            ? expensesOver12MonthsByCategory[category.Key].Sum(x => x.Cost)
                                            : 0.00M;

                var mappedCategory = Mapper.Map<ExpenseTypeList>(category.Value);

                var budgetPlanByCategory = new BudgetPlanExpenseType
                {
                    CurrencySymbol = budgetPlan.CurrencySymbol,
                    ExpenseType = mappedCategory,
                    ExpectedValue = expectedValue,
                    PreviousMonthValue = previousMonthValue,
                    CurrentBudgetPlanValue = currentBudgetPlanValue,
                    AverageMonthValue = averageMonthValue / nbMonthInterval
                };

                budgetPlan.ExpenseTypes.Add(budgetPlanByCategory);
            }

            budgetPlan.ExpensePreviousMonthValue = lastMonthExpenses.Sum(x => x.Cost);
            budgetPlan.ExpenseAverageMonthValue = expensesOver12Months.Sum(x => x.Cost) / nbMonthInterval;
            budgetPlan.ExpenseCurrentBudgetPlanValue = currentBudgetPlan?.ExpenseTypes.Sum(x => x.ExpectedValue);

            var incomes = _incomeRepository.GetList2().Where(x => x.AccountId == accountId).ToList();

            budgetPlan.IncomeCurrentBudgetPlanValue = currentBudgetPlan?.ExpectedIncomes;
            budgetPlan.IncomePreviousMonthValue = incomes.Where(x => previousInterval.IsBetween(x.DateIncome)).Sum(x => x.Cost);
            budgetPlan.IncomeAverageMonthValue = incomes.Where(x => over12MonthsInterval.IsBetween(x.DateIncome)).Sum(x => x.Cost) / nbMonthInterval;

            budgetPlan.ExpectedIncomes = existingBudgetPlan?.ExpectedIncomes ?? budgetPlan.IncomePreviousMonthValue;

            var savings = _savingRepository.GetList2().Where(x => x.AccountId == accountId).ToList();

            budgetPlan.SavingCurrentBudgetPlanValue = currentBudgetPlan?.ExpectedSavings;
            budgetPlan.SavingPreviousMonthValue = savings.Where(x => previousInterval.IsBetween(x.DateSaving)).Sum(x => x.Amount);
            budgetPlan.SavingAverageMonthValue = savings.Where(x => over12MonthsInterval.IsBetween(x.DateSaving)).Sum(x => x.Amount) / nbMonthInterval;

            budgetPlan.ExpectedSavings = existingBudgetPlan?.ExpectedSavings ?? budgetPlan.SavingPreviousMonthValue;

            return budgetPlan;
        }
    }
}