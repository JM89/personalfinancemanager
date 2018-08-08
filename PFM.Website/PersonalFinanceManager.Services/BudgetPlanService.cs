using System;
using System.Collections.Generic;
using PersonalFinanceManager.Models.BudgetPlan;
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Services
{
    public class BudgetPlanService : IBudgetPlanService
    {
        public IList<BudgetPlanListModel> GetBudgetPlans(int accountId)
        {
            throw new NotImplementedException();
        }
        
        public BudgetPlanEditModel GetCurrent(int accountId)
        {
            throw new NotImplementedException();
        }

        public BudgetPlanEditModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void CreateBudgetPlan(BudgetPlanEditModel budgetPlanEditModel, int accountId)
        {
            throw new NotImplementedException();
        }

        public void EditBudgetPlan(BudgetPlanEditModel budgetPlanEditModel, int accountId)
        {
            throw new NotImplementedException();
        }

        public void StartBudgetPlan(int value, int accountId)
        {
            throw new NotImplementedException();
        }

        public void StopBudgetPlan(int value)
        {
            throw new NotImplementedException();
        }

        public BudgetPlanEditModel BuildBudgetPlan(int accountId, int? budgetPlanId = null)
        {
            throw new NotImplementedException();
        }
    }
}