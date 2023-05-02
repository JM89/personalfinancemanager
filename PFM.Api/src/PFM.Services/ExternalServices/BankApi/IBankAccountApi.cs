using Api.Contracts.Shared;
using PFM.Bank.Api.Contracts.Account;
using Refit;

namespace PFM.Services.ExternalServices.BankApi
{
    public interface IBankAccountApi
    {
        [Get("/api/BankAccount/GetList/{userId}")]
        ApiResponse GetList(string userId);

        [Get("/api/BankAccount/Get/{id}")]
        ApiResponse Get(int id);

        [Post("/api/BankAccount/Create/{userId}")]
        ApiResponse Post(string userId, AccountDetails accountDetails);

        [Put("/api/BankAccount/Edit/{id}/{userId}")]
        ApiResponse Put(int id, string userId, AccountDetails accountDetails);

        [Put("/api/BankAccount/Delete/{id}")]
        ApiResponse Delete(int id);

        [Post("/api/SetAsFavorite/{id}")]
        ApiResponse SetAsFavorite(int id);
    }
}
