using PersonalFinanceManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Models.BudgetPlan;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using PersonalFinanceManager.DataAccess;
using PersonalFinanceManager.Models.ExpenditureType;
using PersonalFinanceManager.Models.Dashboard;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.Services.RequestObjects;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities.SearchParameters;
using PersonalFinanceManager.Utils.Helpers;

namespace PersonalFinanceManager.Services
{
    public class BudgetPlanService : IBudgetPlanService
    {
        private readonly IBudgetPlanRepository _budgetPlanRepository;
        private readonly IBudgetByExpenditureTypeRepository _budgetByExpenditureTypeRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IExpenditureRepository _expenditureRepository;
        private readonly IExpenditureTypeRepository _expenditureTypeRepository;
        private readonly IIncomeRepository _incomeRepository;
        private readonly ISavingRepository _savingRepository;

        public BudgetPlanService(IBudgetPlanRepository budgetPlanRepository, IBudgetByExpenditureTypeRepository budgetByExpenditureTypeRepository, IBankAccountRepository bankAccountRepository,
            IExpenditureRepository expenditureRepository, IExpenditureTypeRepository expenditureTypeRepository, IIncomeRepository incomeRepository, 
            ISavingRepository savingRepository)
        {
            this._budgetPlanRepository = budgetPlanRepository;
            this._budgetByExpenditureTypeRepository = budgetByExpenditureTypeRepository;
            this._bankAccountRepository = bankAccountRepository;
            this._expenditureRepository = expenditureRepository;
            this._expenditureTypeRepository = expenditureTypeRepository;
            this._incomeRepository = incomeRepository;
            this._savingRepository = savingRepository;
        }

        /// <summary>
        /// Return the list of budget plans.
        /// </summary>
        /// <returns></returns>
        public IList<BudgetPlanListModel> GetBudgetPlans(int accountId)
        {
            var budgetPlansForAccount = _budgetByExpenditureTypeRepository.GetList2().Where(x => x.AccountId == accountId).Select(x => x.BudgetPlanId);

            var budgetPlans = _budgetPlanRepository.GetList().Where(x => budgetPlansForAccount.Contains(x.Id)).ToList();

            return budgetPlans.Select(Mapper.Map<BudgetPlanListModel>).ToList();
        }
        
        public BudgetPlanEditModel GetCurrent(int accountId)
        {
            var budgetPlansForAccount = _budgetByExpenditureTypeRepository.GetList2().Where(x => x.AccountId == accountId).Select(x => x.BudgetPlanId);

            var currentBudgetPlan = _budgetPlanRepository.GetList().SingleOrDefault(x => budgetPlansForAccount.Contains(x.Id) && x.StartDate.HasValue && !x.EndDate.HasValue);
            if (currentBudgetPlan != null)
            {
                return GetById(currentBudgetPlan.Id);
            }
            return null;
        }

        public BudgetPlanEditModel GetById(int id)
        {
            var budgetPlan = _budgetPlanRepository.GetById(id);
            if (budgetPlan == null)
            {
                return null;
            }

            var budgetPlanExpenditures = _budgetByExpenditureTypeRepository.GetList2(x => x.ExpenditureType).Where(x => x.BudgetPlanId == id);

            var budgetExpenditureTypes = new List<BudgetPlanExpenditureType>();
            foreach(var budgetPlanExpenditure in budgetPlanExpenditures)
            {
                var expenditureType = budgetPlanExpenditure.ExpenditureType;
                var expenditureTypeModel = Mapper.Map<ExpenditureTypeListModel>(expenditureType);
                var budgetExpenditureType = new BudgetPlanExpenditureType()
                {
                    ExpenditureType = expenditureTypeModel,
                    ExpectedValue = budgetPlanExpenditure.Budget
                };
                budgetExpenditureTypes.Add(budgetExpenditureType);
            }

            var budgetPlanModel = Mapper.Map<BudgetPlanEditModel>(budgetPlan);
            budgetPlanModel.ExpenditureTypes = budgetExpenditureTypes.ToList();

            return budgetPlanModel;
        }

        /// <summary>
        /// Create a budget plan.
        /// </summary>
        /// <param name="budgetPlanEditModel"></param>
        /// <param name="accountId"></param>
        public void CreateBudgetPlan(BudgetPlanEditModel budgetPlanEditModel, int accountId)
        {
            var budgetPlanModel = Mapper.Map<BudgetPlanModel>(budgetPlanEditModel);
            _budgetPlanRepository.Create(budgetPlanModel);

            foreach(var expenditureType in budgetPlanEditModel.ExpenditureTypes)
            {
                var plannedExpenditure = new BudgetByExpenditureTypeModel
                {
                    Budget = expenditureType.ExpectedValue,
                    ExpenditureTypeId = expenditureType.ExpenditureType.Id,
                    BudgetPlanId = budgetPlanModel.Id,
                    AccountId = accountId
                };

                // Account ID is defined at BudgetByExpenditureTypeModel time because at first, I wanted to plan budget through several accounts.
                // It might evolve later so for now, I keep it here, independently from BudgetByExpenditureTypeModel.

                _budgetByExpenditureTypeRepository.Create(plannedExpenditure);
            }
        }

        /// <summary>
        /// Edit a budget plan.
        /// </summary>
        /// <param name="budgetPlanEditModel"></param>
        /// <param name="accountId"></param>
        public void EditBudgetPlan(BudgetPlanEditModel budgetPlanEditModel, int accountId)
        {
            var budgetPlan = _budgetPlanRepository.GetById(budgetPlanEditModel.Id);
            budgetPlan.Name = budgetPlanEditModel.Name;
            budgetPlan.ExpectedIncomes = budgetPlanEditModel.ExpectedIncomes;
            budgetPlan.ExpectedSavings = budgetPlanEditModel.ExpectedSavings;
            _budgetPlanRepository.Update(budgetPlan);

            var existingBudgetPlanExpenditures = _budgetByExpenditureTypeRepository.GetList()
                  .Include(x => x.ExpenditureType)
                  .Where(x => x.BudgetPlanId == budgetPlanEditModel.Id);

            foreach (var budgetExpenditureType in budgetPlanEditModel.ExpenditureTypes)
            {
                var existingBudgetExpenditureType = existingBudgetPlanExpenditures.SingleOrDefault(x => x.ExpenditureType.Id == budgetExpenditureType.ExpenditureType.Id);

                if (existingBudgetExpenditureType == null)
                {
                    // Add
                    var plannedExpenditure = new BudgetByExpenditureTypeModel()
                    {
                        Budget = budgetExpenditureType.ExpectedValue,
                        ExpenditureTypeId = budgetExpenditureType.ExpenditureType.Id,
                        BudgetPlanId = budgetPlanEditModel.Id,
                        AccountId = accountId
                    };
                    _budgetByExpenditureTypeRepository.Create(plannedExpenditure);
                }
                else
                {
                    // Update
                    existingBudgetExpenditureType.Budget = budgetExpenditureType.ExpectedValue;
                    _budgetByExpenditureTypeRepository.Update(existingBudgetExpenditureType);
                }
            }
        }

        public void StartBudgetPlan(int value, int accountId)
        {
            var budgetPlansForAccount = _budgetByExpenditureTypeRepository.GetList().Where(x => x.AccountId == accountId).Select(x => x.BudgetPlanId);
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

        public BudgetPlanEditModel BuildBudgetPlan(int accountId, int? budgetPlanId = null)
        {
            var currencySymbol = _bankAccountRepository.GetById(accountId, x => x.Currency).Currency.Symbol;

            var today = DateTime.Now;
            var over12MonthsInterval = new Interval(today, DateTimeUnitEnums.Years, 1);
            var previousInterval = new Interval(today, DateTimeUnitEnums.Months, 1);
            var firstOfNextMonth = DateTimeFormatHelper.GetFirstDayOfMonth(today.AddMonths(1));

            // Retrieve the categories
            var categories = _expenditureTypeRepository.GetList2().GroupBy(x => x.Id).ToDictionary(x => x.Key, y => y.Single());

            // Retrieve the expenses over the last 12 months (excluding current month) 
            var expensesOver12Months = _expenditureRepository.GetByParameters(new ExpenditureGetListSearchParameters()
            {
                AccountId = accountId,
                StartDate = over12MonthsInterval.StartDate,
                EndDate = over12MonthsInterval.EndDate
            });

            // Group by category the expenses over the last 12 months
            var expensesOver12MonthsByCategory = expensesOver12Months.GroupBy(x => x.TypeExpenditureId).ToDictionary(x => x.Key, y => y.ToList());

            over12MonthsInterval.StartDate = DateTimeFormatHelper.GetFirstDayOfMonth(
              expensesOver12Months.Any() ?
              expensesOver12Months.OrderBy(x => x.DateExpenditure).First().DateExpenditure :
              today);

            var nbMonthInterval = over12MonthsInterval.Count(DateTimeUnitEnums.Months);
            if (nbMonthInterval == 0)
                nbMonthInterval = 1; // No expenses -> no division by zero

            // Retrieve the expenses last months and group by category
            var lastMonthExpenses = expensesOver12Months.Where(x => previousInterval.IsBetween(x.DateExpenditure)).ToList();
            var lastMonthExpensesByCategory = lastMonthExpenses.GroupBy(x => x.TypeExpenditureId).ToDictionary(x => x.Key, y => y.ToList());

            // Get the current Budget Plan for the account. If none, returns a default of cost of 0.00
            var currentBudgetPlan = GetCurrent(accountId);
            var currentBudgetPlanByCategory = currentBudgetPlan?.ExpenditureTypes
                                                    .GroupBy(x => x.ExpenditureType.Id)
                                                    .ToDictionary(x => x.Key, y => y.Single().ExpectedValue);

            // Get the existing Budget Plan for the provided ID. If none, returns a default of cost of 0.00
            var existingBudgetPlan = budgetPlanId.HasValue ? GetById(budgetPlanId.Value) : null;
            var existingBudgetPlanByCategory = existingBudgetPlan?.ExpenditureTypes.GroupBy(x => x.ExpenditureType.Id).ToDictionary(x => x.Key, y => y.Single().ExpectedValue);

            BudgetPlanEditModel budgetPlan = null;

            if (existingBudgetPlan != null)
            {
                budgetPlan = new BudgetPlanEditModel()
                {
                    Id = existingBudgetPlan.Id,
                    Name = existingBudgetPlan.Name,
                    ExpenditureTypes = new List<BudgetPlanExpenditureType>(),
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
                budgetPlan = new BudgetPlanEditModel()
                {
                    ExpenditureTypes = new List<BudgetPlanExpenditureType>(),
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

                var categoryModel = Mapper.Map<ExpenditureTypeListModel>(category.Value);

                var budgetPlanByCategory = new BudgetPlanExpenditureType
                {
                    CurrencySymbol = budgetPlan.CurrencySymbol,
                    ExpenditureType = categoryModel,
                    ExpectedValue = expectedValue,
                    PreviousMonthValue = previousMonthValue,
                    CurrentBudgetPlanValue = currentBudgetPlanValue,
                    AverageMonthValue = averageMonthValue / nbMonthInterval
                };

                budgetPlan.ExpenditureTypes.Add(budgetPlanByCategory);
            }

            budgetPlan.ExpenditurePreviousMonthValue = lastMonthExpenses.Sum(x => x.Cost);
            budgetPlan.ExpenditureAverageMonthValue = expensesOver12Months.Sum(x => x.Cost) / nbMonthInterval;
            budgetPlan.ExpenditureCurrentBudgetPlanValue = currentBudgetPlan?.ExpenditureTypes.Sum(x => x.ExpectedValue);

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