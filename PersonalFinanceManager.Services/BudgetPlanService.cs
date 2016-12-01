﻿using PersonalFinanceManager.Models;
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

namespace PersonalFinanceManager.Services
{
    public class BudgetPlanService : IBudgetPlanService
    {
        ApplicationDbContext db;

        public BudgetPlanService()
        {
            db = new ApplicationDbContext();
        }

        /// <summary>
        /// Return the list of budget plans.
        /// </summary>
        /// <returns></returns>
        public IList<BudgetPlanListModel> GetBudgetPlans()
        {
            var budgetPlans = db.BudgetPlanModels.ToList();

            return budgetPlans.Select(x => Mapper.Map<BudgetPlanListModel>(x)).ToList();
        }

        //public IList<BudgetPlanListModel> GetBudgetPlans(BudgetPlanSearch search)
        //{
        //    var budgetPlans = db.BudgetByExpenditureTypeModels
        //        .Include(x => x.BudgetPlan)
        //        .Where(x => (search.AccountId.HasValue && x.AccountId == search.AccountId.Value)
        //            && (search.StartDate.HasValue && x.BudgetPlan.StartDate.HasValue && x.BudgetPlan.StartDate >= search.StartDate.Value)
        //            && (search.EndDate.HasValue && !x.BudgetPlan.EndDate.HasValue || x.BudgetPlan.EndDate < search.EndDate.Value)).ToList();

        //    var mappedBudgetPlans = budgetPlans.Select(x => new BudgetPlanListModel()
        //    {

        //    });
        //    //{
        //    //    ExpenditureId = x.ExpenditureTypeId,
        //    //    StartDate = x.BudgetPlan.StartDate.Value,
        //    //    EndDate = x.BudgetPlan.EndDate.HasValue ? x.BudgetPlan.EndDate.Value : search.EndDate.Value,
        //    //    ExpectedValue = x.Budget
        //    //});

        //    return mappedBudgetPlans.ToList();
        //}

        public BudgetPlanEditModel GetCurrent()
        {
            var currentBudgetPlan = db.BudgetPlanModels.SingleOrDefault(x => !x.EndDate.HasValue);
            if (currentBudgetPlan != null)
            {
                return GetById(currentBudgetPlan.Id);
            }
            return null;
        }

        public BudgetPlanEditModel GetById(int id)
        {
            var budgetPlan = db.BudgetPlanModels.SingleOrDefault(x => x.Id == id);
            if (budgetPlan == null)
            {
                return null;
            }

            var budgetPlanExpenditures = db.BudgetByExpenditureTypeModels
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
            db.BudgetPlanModels.Add(budgetPlanModel);
            db.SaveChanges();

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

                db.BudgetByExpenditureTypeModels.Add(plannedExpenditure);
            }
            db.SaveChanges();
        }

        /// <summary>
        /// Edit a budget plan.
        /// </summary>
        /// <param name="budgetPlanEditModel"></param>
        public void EditBudgetPlan(BudgetPlanEditModel budgetPlanEditModel, int accountId)
        {
            var budgetPlan = db.BudgetPlanModels.SingleOrDefault(x => x.Id == budgetPlanEditModel.Id);
            budgetPlan.Name = budgetPlanEditModel.Name;

            var existingBudgetPlanExpenditures = db.BudgetByExpenditureTypeModels
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
                    db.BudgetByExpenditureTypeModels.Add(plannedExpenditure);
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

            db.SaveChanges();
        }

        public void StartBudgetPlan(int value)
        {
            var currentBudgetPlan = db.BudgetPlanModels.SingleOrDefault(x => x.Id != value && !x.EndDate.HasValue);
            if (currentBudgetPlan != null)
            {
                currentBudgetPlan.EndDate = DateTime.Now;
            }

            var budgetPlan = db.BudgetPlanModels.Single(x => x.Id == value);
            var nextMonth = DateTime.Now.AddMonths(1);
            var firstOfNextMonth = new DateTime(nextMonth.Year,nextMonth.Month, 1);
            budgetPlan.StartDate = firstOfNextMonth;

            db.SaveChanges();
        }

        public void StopBudgetPlan(int value)
        {
            var budgetPlan = db.BudgetPlanModels.Single(x => x.Id == value);
            budgetPlan.EndDate = DateTime.Now;
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }


    }
}