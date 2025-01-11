using Refit;
using System.Threading.Tasks;
using PFM.Bank.Api.Contracts.Shared;

namespace PFM.Services.ExternalServices.BankApi
{
    public interface ICurrencyApi
    {
        [Get("/api/Currency/GetList")]
        Task<ApiResponse> GetList();

        [Get("/api/Currency/Get/{id}")]
        Task<ApiResponse> Get(int id);
    }
}
