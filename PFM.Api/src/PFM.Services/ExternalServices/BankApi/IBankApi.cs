using Api.Contracts.Shared;
using PFM.Bank.Api.Contracts.Bank;
using Refit;
using System.Threading.Tasks;

namespace PFM.Services.ExternalServices.BankApi
{
    public interface IBankApi
    {
        [Get("/api/Bank/GetList/{userId}")]
        Task<ApiResponse> GetList(string userId);

        [Get("/api/Bank/Get/{id}")]
        Task<ApiResponse> Get(int id);

        [Post("/api/Bank/Create")]
        Task<ApiResponse> Post(BankDetails bankDetails);

        [Put("/api/Bank/Edit/{id}")]
        Task<ApiResponse> Put(int id, BankDetails bankDetails);

        [Put("/api/Bank/Delete/{id}")]
        Task<ApiResponse> Delete(int id);
    }
}
