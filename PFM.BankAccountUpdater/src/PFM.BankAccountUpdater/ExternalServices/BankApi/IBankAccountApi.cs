using Api.Contracts.Shared;
using PFM.Bank.Api.Contracts.Account;
using Refit;

namespace PFM.BankAccountUpdater.ExternalServices.BankApi
{
    public interface IBankAccountApi
    {
        [Get("/api/BankAccount/Get/{id}")]
        Task<ApiResponse> Get(int id);

        [Put("/api/BankAccount/Edit/{id}/{userId}")]
        Task<ApiResponse> Edit(int id, string userId, AccountDetails accountDetails);
    }
}
