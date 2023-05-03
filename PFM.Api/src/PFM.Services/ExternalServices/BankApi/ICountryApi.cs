using Api.Contracts.Shared;
using Refit;
using System.Threading.Tasks;

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
