using System.Collections.Generic;
using PersonalFinanceManager.Models.BudgetPlan;
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IBudgetPlanService : IBaseService
    {
        IList<BudgetPlanListModel> GetBudgetPlans(int accountId);

        BudgetPlanEditModel GetCurrent(int accountId);

        BudgetPlanEditModel GetById(int id);

        void CreateBudgetPlan(BudgetPlanEditModel budgetPlanEditModel, int accountId);

        void EditBudgetPlan(BudgetPlanEditModel budgetPlanEditModel, int accountId);

        void StartBudgetPlan(int value, int accountId);

        void StopBudgetPlan(int value);

        BudgetPlanEditModel BuildBudgetPlan(int accountId, int? budgetPlanId = null);
    }
}