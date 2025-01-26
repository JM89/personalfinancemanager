using PFM.Api.Contracts.Shared;
using Refit;

namespace PFM.Services.ExternalServices
{
    public interface ICurrencyApi
    {
        [Get("/api/Currency/GetList")]
        Task<ApiResponse> GetList();

        [Get("/api/Currency/Get/{id}")]
        Task<ApiResponse> Get(int id);
    }
}
