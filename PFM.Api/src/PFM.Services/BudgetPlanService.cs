using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PFM.Services.Interfaces;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;
using PFM.Api.Contracts.BudgetPlan;
using PFM.Api.Contracts.ExpenseType;
using System.Threading.Tasks;

namespace PFM.Services
{
    public class BudgetPlanService(
        IBudgetPlanRepository budgetPlanRepository,
        IBudgetByExpenseTypeRepository budgetByExpenseTypeRepository)
        : IBudgetPlanService
    {
        /// <summary>
        /// Return the list of budget plans.
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<BudgetPlanList>> GetBudgetPlans(int accountId)
        {
            var budgetPlansForAccount = budgetByExpenseTypeRepository.GetList2().Where(x => x.AccountId == accountId).Select(x => x.BudgetPlanId);

            var budgetPlans = budgetPlanRepository.GetList().Where(x => budgetPlansForAccount.Contains(x.Id)).ToList();
            
            return Task.FromResult(budgetPlans.Select(Mapper.Map<BudgetPlanList>));
        }
        
        public async Task<BudgetPlanDetails> GetCurrent(int accountId)
        {
            var budgetPlansForAccount = budgetByExpenseTypeRepository.GetList2().Where(x => x.AccountId == accountId).Select(x => x.BudgetPlanId);

            var currentBudgetPlan = budgetPlanRepository.GetList().SingleOrDefault(x => budgetPlansForAccount.Contains(x.Id) && x.StartDate.HasValue && !x.EndDate.HasValue);
            if (currentBudgetPlan != null)
            {
                return await GetById(currentBudgetPlan.Id);
            }
            return null;
        }

        public Task<BudgetPlanDetails> GetById(int id)
        {
            var budgetPlan = budgetPlanRepository.GetById(id);
            if (budgetPlan == null)
            {
                return null;
            }

            var budgetPlanExpenses = budgetByExpenseTypeRepository.GetList2(x => x.ExpenseType).Where(x => x.BudgetPlanId == id);

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

            return Task.FromResult(mappedBudgetPlan);
        }

        /// <summary>
        /// Create a budget plan.
        /// </summary>
        /// <param name="budgetPlanDetails"></param>
        /// <param name="accountId"></param>
        public Task<bool> CreateBudgetPlan(BudgetPlanDetails budgetPlanDetails, int accountId)
        {
            var budgetPlan = Mapper.Map<BudgetPlan>(budgetPlanDetails);
            budgetPlanRepository.Create(budgetPlan);

            foreach(var expenseType in budgetPlanDetails.ExpenseTypes)
            {
                var plannedExpense = new BudgetByExpenseType
                {
                    Budget = expenseType.ExpectedValue,
                    ExpenseTypeId = expenseType.ExpenseType.Id,
                    BudgetPlanId = budgetPlan.Id,
                    AccountId = accountId
                };

                // Account ID is defined at BudgetByExpenseType time because at first, I wanted to plan budget through several accounts.
                // It might evolve later so for now, I keep it here, independently from BudgetByExpenseType.

                budgetByExpenseTypeRepository.Create(plannedExpense);
            }
            
            return Task.FromResult(true);
        }

        /// <summary>
        /// Edit a budget plan.
        /// </summary>
        /// <param name="budgetPlanDetails"></param>
        /// <param name="accountId"></param>
        public Task<bool> EditBudgetPlan(BudgetPlanDetails budgetPlanDetails, int accountId)
        {
            var budgetPlan = budgetPlanRepository.GetById(budgetPlanDetails.Id);
            budgetPlan.Name = budgetPlanDetails.Name;
            budgetPlan.ExpectedIncomes = budgetPlanDetails.ExpectedIncomes;
            budgetPlan.ExpectedSavings = budgetPlanDetails.ExpectedSavings;
            budgetPlanRepository.Update(budgetPlan);

            var existingBudgetPlanExpenses = budgetByExpenseTypeRepository.GetList2(x => x.ExpenseType)
                  .Where(x => x.BudgetPlanId == budgetPlanDetails.Id).ToList();

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
                    budgetByExpenseTypeRepository.Create(plannedExpense);
                }
                else
                {
                    // Update
                    existingBudgetExpenseType.Budget = budgetExpenseType.ExpectedValue;
                    budgetByExpenseTypeRepository.Update(existingBudgetExpenseType);
                }
            }
            
            return Task.FromResult(true);
        }

        public Task<bool> StartBudgetPlan(int value, int accountId)
        {
            var budgetPlansForAccount = budgetByExpenseTypeRepository.GetList().Where(x => x.AccountId == accountId).Select(x => x.BudgetPlanId);
            var currentBudgetPlan = budgetPlanRepository.GetList().SingleOrDefault(x => budgetPlansForAccount.Contains(x.Id) && x.StartDate.HasValue && !x.EndDate.HasValue);
            if (currentBudgetPlan != null)
            {
                currentBudgetPlan.EndDate = DateTime.Now;
                budgetPlanRepository.Update(currentBudgetPlan);
            }

            var budgetPlan = budgetPlanRepository.GetById(value);
            var nextMonth = DateTime.Now.AddMonths(1);
            var firstOfNextMonth = new DateTime(nextMonth.Year,nextMonth.Month, 1);
            budgetPlan.StartDate = firstOfNextMonth;

            budgetPlanRepository.Update(budgetPlan);

            return Task.FromResult(true);
        }

        public Task<bool> StopBudgetPlan(int value)
        {
            var budgetPlan = budgetPlanRepository.GetById(value);
            budgetPlan.EndDate = DateTime.Now;
            budgetPlanRepository.Update(budgetPlan);
            return Task.FromResult(true);
        }
    }
}