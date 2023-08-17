using Api.Contracts.Shared;
using PFM.Api.Contracts.ExpenseType;
using Refit;

namespace PFM.Website.ExternalServices
{
	public interface IExpenseTypeApi
	{
        [Get("/api/ExpenseType/GetList")]
        Task<ApiResponse> Get();

        [Get("/api/ExpenseType/Get/{id}")]
        Task<ApiResponse> Get(int id);

        [Post("/api/ExpenseType/Create/")]
        Task<ApiResponse> Create(ExpenseTypeDetails obj);

        [Put("/api/ExpenseType/Edit/{id}")]
        Task<ApiResponse> Edit(int id, ExpenseTypeDetails obj);

        [Delete("/api/ExpenseType/Delete/{id}")]
        Task<ApiResponse> Delete(int id);
    }
}

