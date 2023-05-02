using Api.Contracts.Shared;
using PFM.Bank.Api.Contracts.Bank;
using Refit;

namespace PFM.Services.ExternalServices.BankApi
{
    public interface IBankApi
    {
        [Get("/api/Bank/GetList/{userId}")]
        ApiResponse GetList(string userId);

        [Get("/api/Bank/Get/{id}")]
        ApiResponse Get(int id);

        [Post("/api/Bank/Create")]
        ApiResponse Post(BankDetails bankDetails);

        [Put("/api/Bank/Edit/{id}")]
        ApiResponse Put(int id, BankDetails bankDetails);

        [Put("/api/Bank/Delete/{id}")]
        ApiResponse Delete(int id);
    }
}
