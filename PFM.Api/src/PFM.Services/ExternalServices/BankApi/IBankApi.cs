using Api.Contracts.Shared;
using PFM.Bank.Api.Contracts.Bank;
using Refit;
using System.Threading.Tasks;

namespace PFM.Services.ExternalServices.BankApi
{
    public interface IBankApi
    {
        [Get("/api/Bank/GetList")]
        Task<ApiResponse> GetList();

        [Get("/api/Bank/Get/{id}")]
        Task<ApiResponse> Get(int id);

        [Post("/api/Bank/Create")]
        Task<ApiResponse> Create(BankDetails bankDetails);

        [Put("/api/Bank/Edit/{id}")]
        Task<ApiResponse> Edit(int id, BankDetails bankDetails);

        [Delete("/api/Bank/Delete/{id}")]
        Task<ApiResponse> Delete(int id);
    }
}
