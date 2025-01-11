using Refit;
using System.Threading.Tasks;
using PFM.Bank.Api.Contracts.Shared;

namespace PFM.Services.ExternalServices.BankApi
{
    public interface ICountryApi
    {
        [Get("/api/Country/GetList")]
        Task<ApiResponse> GetList();

        [Get("/api/Country/Get/{id}")]
        Task<ApiResponse> Get(int id);
    }
}
