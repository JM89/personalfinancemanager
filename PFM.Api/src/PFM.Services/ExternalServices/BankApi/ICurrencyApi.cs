using Api.Contracts.Shared;
using Refit;

namespace PFM.Services.ExternalServices.BankApi
{
    public interface ICurrencyApi
    {
        [Get("/api/Currency/GetList/{userId}")]
        ApiResponse GetList();

        [Get("/api/Currency/Get/{id}")]
        ApiResponse Get(int id);
    }
}
