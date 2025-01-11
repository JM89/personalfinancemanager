using PFM.Bank.Api.Contracts.Bank;
using Refit;
using System.Threading.Tasks;
using PFM.Bank.Api.Contracts.Shared;

namespace PFM.Services.ExternalServices.BankApi
{
    public interface IBankApi
    {
        [Get("/api/Bank/GetList/{userId}")]
        Task<ApiResponse> GetList(string userId);

        [Get("/api/Bank/Get/{id}")]
        Task<ApiResponse> Get(int id);

        [Post("/api/Bank/Create/{userId}")]
        Task<ApiResponse> Create(string userId, BankDetails bankDetails);

        [Put("/api/Bank/Edit/{id}/{userId}")]
        Task<ApiResponse> Edit(int id, string userId, BankDetails bankDetails);

        [Delete("/api/Bank/Delete/{id}")]
        Task<ApiResponse> Delete(int id);
    }
}
