using Api.Contracts.Shared;
using PFM.Api.Contracts.Expense;
using PFM.Api.Contracts.SearchParameters;
using Refit;

namespace PFM.Website.ExternalServices
{
	public interface IExpenseApi
	{
        [Get("/api/Expense/GetList/{accountId}")]
        Task<ApiResponse> GetList(ExpenseGetListSearchParameters search);

        [Get("/api/Expense/Get/{id}")]
        Task<ApiResponse> Get(int id);

        [Post("/api/Expense/Create/")]
        Task<ApiResponse> Create(ExpenseDetails obj);

        [Delete("/api/Expense/Delete/{id}")]
        Task<ApiResponse> Delete(int id);

        [Post("/api/Expense/ChangeDebitStatus/{id}")]
        Task<ApiResponse> ChangeDebitStatus(int id, bool debitStatus);
    }
}

