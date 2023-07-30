using Api.Contracts.Shared;
using PFM.Bank.Api.Contracts.Bank;
using Refit;

namespace PFM.Website.ExternalServices
{
	public interface IBankApi
	{
        [Get("/api/Bank/GetList")]
        Task<ApiResponse> Get();

        [Get("/api/Bank/Get/{id}")]
        Task<ApiResponse> Get(int id);

        [Post("/api/Bank/Create/")]
        Task<ApiResponse> Create(BankDetails obj);

        [Put("/api/Bank/Edit/{id}")]
        Task<ApiResponse> Edit(int id, BankDetails obj);

        [Delete("/api/Bank/Delete/{id}")]
        Task<ApiResponse> Delete(int id);
    }
}

