using Api.Contracts.Shared;
using Refit;

namespace PFM.Services.ExternalServices.BankApi
{
    public interface ICountryApi
    {
        [Get("/api/Country/GetList/{userId}")]
        ApiResponse GetList();

        [Get("/api/Country/Get/{id}")]
        ApiResponse Get(int id);
    }
}
