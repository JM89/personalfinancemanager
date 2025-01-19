using PFM.Api.Contracts.Shared;
using PFM.Api.Contracts.ExpenseType;
using Refit;

namespace PFM.Website.ExternalServices
{
	public interface IExpenseTypeApi
	{
        [Get("/api/ExpenseType/GetList/{userId}")]
        Task<ApiResponse> Get(string userId);

        [Get("/api/ExpenseType/Get/{id}")]
        Task<ApiResponse> Get(int id);

        [Post("/api/ExpenseType/Create/{userId}")]
        Task<ApiResponse> Create(string userId, ExpenseTypeDetails obj);

        [Put("/api/ExpenseType/Edit/{id}/{userId}")]
        Task<ApiResponse> Edit(int id, string userId, ExpenseTypeDetails obj);

        [Delete("/api/ExpenseType/Delete/{id}")]
        Task<ApiResponse> Delete(int id);
    }
}

