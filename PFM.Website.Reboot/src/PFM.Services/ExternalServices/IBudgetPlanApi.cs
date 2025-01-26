using PFM.Api.Contracts.BudgetPlan;
using PFM.Api.Contracts.Shared;
using Refit;

namespace PFM.Services.ExternalServices
{
	public interface IBudgetPlanApi
	{
        [Get("/api/BudgetPlan/GetList/{accountId}")]
        Task<ApiResponse> GetList(int accountId);

        [Get("/api/BudgetPlan/Get/{id}")]
        Task<ApiResponse> Get(int id);

        [Post("/api/BudgetPlan/Create/{accountId}")]
        Task<ApiResponse> Create(int accountId, BudgetPlanDetails obj);

        [Put("/api/BudgetPlan/Edit/{accountId}")]
        Task<ApiResponse> Edit(int accountId, BudgetPlanDetails obj);

        [Post("/api/BudgetPlan/Start/{value}/{accountId}")]
        Task<ApiResponse> Start(int value, int accountId);

        [Post("/api/BudgetPlan/Stop/{value}")]
        Task<ApiResponse> Stop(int value);
    }
}

