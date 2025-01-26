using PFM.Api.Contracts.Shared;
using PFM.Bank.Api.Contracts.Bank;
using Refit;

namespace PFM.Services.ExternalServices
{
	public interface IBankApi
	{
        [Get("/api/Bank/GetList/{userId}")]
        Task<ApiResponse> Get(string userId);

        [Get("/api/Bank/Get/{id}")]
        Task<ApiResponse> Get(int id);

        [Post("/api/Bank/Create/{userId}")]
        Task<ApiResponse> Create(string userId, BankDetails obj);

        [Put("/api/Bank/Edit/{id}/{userId}")]
        Task<ApiResponse> Edit(int id, string userId, BankDetails obj);

        [Delete("/api/Bank/Delete/{id}")]
        Task<ApiResponse> Delete(int id);
    }
}

