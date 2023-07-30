using Api.Contracts.Shared;
using Refit;

namespace PFM.Website.ExternalServices
{
	public interface ICountryApi
	{
        [Get("/api/Country/GetList")]
        Task<ApiResponse> Get();

        [Get("/api/Country/Get/{id}")]
        Task<ApiResponse> Get(int id);
    }
}

