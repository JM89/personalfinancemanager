using Api.Contracts.Shared;
using Refit;
using System.Threading.Tasks;

namespace PFM.Services.ExternalServices.BankApi
{
    public interface ICurrencyApi
    {
        [Get("/api/Currency/GetList/{userId}")]
        Task<ApiResponse> GetList();

        [Get("/api/Currency/Get/{id}")]
        Task<ApiResponse> Get(int id);
    }
}
