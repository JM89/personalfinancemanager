﻿using System.Collections.Generic;
using System.Threading.Tasks;
using PFM.Api.Contracts.BudgetPlan;

namespace PFM.Services.Interfaces
{
    public interface IBudgetPlanService : IBaseService
    {
        IList<BudgetPlanList> GetBudgetPlans(int accountId);

        BudgetPlanDetails GetCurrent(int accountId);

        BudgetPlanDetails GetById(int id);

        void CreateBudgetPlan(BudgetPlanDetails budgetPlanDetails, int accountId);

        void EditBudgetPlan(BudgetPlanDetails budgetPlanDetails, int accountId);

        void StartBudgetPlan(int value, int accountId);

        void StopBudgetPlan(int value);

        Task<BudgetPlanDetails> BuildBudgetPlan(int accountId, int? budgetPlanId = null);
    }
}