using System.Collections.Generic;
using System.Threading.Tasks;
using PersonalFinanceManager.Models.BudgetPlan;
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IBudgetPlanService : IBaseService
    {
        Task<IList<BudgetPlanListModel>> GetBudgetPlans(int accountId);

        Task<BudgetPlanEditModel> GetCurrent(int accountId);

        Task<BudgetPlanEditModel> GetById(int id);

        Task<bool> CreateBudgetPlan(BudgetPlanEditModel budgetPlanEditModel, int accountId);

        Task<bool> EditBudgetPlan(BudgetPlanEditModel budgetPlanEditModel, int accountId);

        Task<bool> StartBudgetPlan(int value, int accountId);

        Task<bool> StopBudgetPlan(int value);

        Task<BudgetPlanEditModel> BuildBudgetPlan(int accountId, int? budgetPlanId = null);
    }
}