using PFM.Api.Contracts.BudgetPlan;
using PFM.Api.Contracts.Dashboard;
using System;
using System.Threading.Tasks;

namespace PFM.Services.Interfaces
{
    public interface IMovementSummaryService : IBaseService
    {
        Task<ExpenseSummary> GetExpenseSummary(int accountId, BudgetPlanDetails budgetPlan, DateTime referenceDate);
    }
}