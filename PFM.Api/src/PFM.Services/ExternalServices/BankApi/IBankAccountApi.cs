using Api.Contracts.Shared;
using PFM.Bank.Api.Contracts.Account;
using Refit;
using System.Threading.Tasks;

namespace PFM.Services.ExternalServices.BankApi
{
    public interface IBankAccountApi
    {
        [Get("/api/BankAccount/GetList/{userId}")]
        Task<ApiResponse> GetList(string userId);

        [Get("/api/BankAccount/Get/{id}")]
        Task<ApiResponse> Get(int id);

        [Post("/api/BankAccount/Create/{userId}")]
        Task<ApiResponse> Post(string userId, AccountDetails accountDetails);

        [Put("/api/BankAccount/Edit/{id}/{userId}")]
        Task<ApiResponse> Put(int id, string userId, AccountDetails accountDetails);

        [Put("/api/BankAccount/Delete/{id}")]
        Task<ApiResponse> Delete(int id);

        [Post("/api/SetAsFavorite/{id}")]
        Task<ApiResponse> SetAsFavorite(int id);
    }
}
