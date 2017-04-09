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

namespace PersonalFinanceManager.Services
{
    public class BudgetPlanService : IBudgetPlanService
    {
        private readonly IBudgetPlanRepository _budgetPlanRepository;
        private readonly IBudgetByExpenditureTypeRepository _budgetByExpenditureTypeRepository;

        public BudgetPlanService(IBudgetPlanRepository budgetPlanRepository, IBudgetByExpenditureTypeRepository budgetByExpenditureTypeRepository)
        {
            this._budgetPlanRepository = budgetPlanRepository;
            this._budgetByExpenditureTypeRepository = budgetByExpenditureTypeRepository;
        }

        /// <summary>
        /// Return the list of budget plans.
        /// </summary>
        /// <returns></returns>
        public IList<BudgetPlanListModel> GetBudgetPlans(int accountId)
        {
            var budgetPlansForAccount = _budgetByExpenditureTypeRepository.GetList().Where(x => x.AccountId == accountId).ToList().Select(x => x.BudgetPlanId);

            var budgetPlans = _budgetPlanRepository.GetList().Where(x => budgetPlansForAccount.Contains(x.Id)).ToList();

            return budgetPlans.Select(x => Mapper.Map<BudgetPlanListModel>(x)).ToList();
        }
        
        public BudgetPlanEditModel GetCurrent(int accountId)
        {
            var budgetPlansForAccount = _budgetByExpenditureTypeRepository.GetList().Where(x => x.AccountId == accountId).ToList().Select(x => x.BudgetPlanId);

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

            var budgetPlanExpenditures = _budgetByExpenditureTypeRepository.GetList()
                .Include(x => x.ExpenditureType)
                .Where(x => x.BudgetPlanId == id);

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
        public void CreateBudgetPlan(BudgetPlanEditModel budgetPlanEditModel, int accountId)
        {
            var budgetPlanModel = Mapper.Map<BudgetPlanModel>(budgetPlanEditModel);
            _budgetPlanRepository.Create(budgetPlanModel);

            var plannedExpenditures = new List<BudgetByExpenditureTypeModel>();
            foreach(var expenditureType in budgetPlanEditModel.ExpenditureTypes)
            {
                var plannedExpenditure = new BudgetByExpenditureTypeModel();
                plannedExpenditure.Budget = expenditureType.ExpectedValue;
                plannedExpenditure.ExpenditureTypeId = expenditureType.ExpenditureType.Id;
                plannedExpenditure.BudgetPlanId = budgetPlanModel.Id;

                // Account ID is defined at BudgetByExpenditureTypeModel time because at first, I wanted to plan budget through several accounts.
                // It might evolve later so for now, I keep it here, independently from BudgetByExpenditureTypeModel.
                plannedExpenditure.AccountId = accountId;

                _budgetByExpenditureTypeRepository.Create(plannedExpenditure);
            }
        }

        /// <summary>
        /// Edit a budget plan.
        /// </summary>
        /// <param name="budgetPlanEditModel"></param>
        public void EditBudgetPlan(BudgetPlanEditModel budgetPlanEditModel, int accountId)
        {
            var budgetPlan = _budgetPlanRepository.GetById(budgetPlanEditModel.Id);
            budgetPlan.Name = budgetPlanEditModel.Name;

            var existingBudgetPlanExpenditures = _budgetByExpenditureTypeRepository.GetList()
                  .Include(x => x.ExpenditureType)
                  .Where(x => x.BudgetPlanId == budgetPlanEditModel.Id);

            //var existingIds = existingBudgetPlanExpenditures.Select(x => x.ExpenditureType.Id).ToList();

            var plannedExpenditures = new List<BudgetByExpenditureTypeModel>();
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
                }
                //existingIds.Remove(budgetExpenditureType.ExpenditureType.Id);
            }

            // Disable the Deletion of expenditures if used in budget
            //foreach (var deletedExpenditureId in existingIds)
            //{
            //    var budgetExpenditureTypeToRemove = db.BudgetByExpenditureTypeModels.Single(x => x.BudgetPlanId == budgetPlanEditModel.Id && x.ExpenditureTypeId == deletedExpenditureId);
            //    db.BudgetByExpenditureTypeModels.Remove(budgetExpenditureTypeToRemove);
            //}
        }

        public void StartBudgetPlan(int value, int accountId)
        {
            var currentBudgetPlan = _budgetPlanRepository.GetList().SingleOrDefault(x => x.Id != value && !x.EndDate.HasValue);
            var accountBudgetPlanExp = _budgetByExpenditureTypeRepository.GetList().Any(x => x.AccountId == accountId && x.BudgetPlanId == currentBudgetPlan.Id);
            if (currentBudgetPlan != null && accountBudgetPlanExp)
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
    }
}