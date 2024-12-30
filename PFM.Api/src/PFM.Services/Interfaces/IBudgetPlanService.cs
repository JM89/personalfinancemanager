using System.Collections.Generic;
using System.Threading.Tasks;
using PFM.Api.Contracts.BudgetPlan;

namespace PFM.Services.Interfaces
{
    public interface IBudgetPlanService : IBaseService
    {
        Task<IEnumerable<BudgetPlanList>> GetBudgetPlans(int accountId);

        Task<BudgetPlanDetails> GetCurrent(int accountId);

        Task<BudgetPlanDetails> GetById(int id);

        Task<bool> CreateBudgetPlan(BudgetPlanDetails budgetPlanDetails, int accountId);

        Task<bool> EditBudgetPlan(BudgetPlanDetails budgetPlanDetails, int accountId);

        Task<bool> StartBudgetPlan(int value, int accountId);

        Task<bool> StopBudgetPlan(int value);
    }
}