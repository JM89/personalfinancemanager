using PFM.Api.Contracts.Income;
using PFM.Api.Contracts.Shared;
using Refit;

namespace PFM.Services.ExternalServices
{
	public interface IIncomeApi
	{
        [Get("/api/Income/GetList/{accountId}")]
        Task<ApiResponse> GetList(int accountId);

        [Get("/api/Income/Get/{id}")]
        Task<ApiResponse> Get(int id);

        [Post("/api/Income/Create/")]
        Task<ApiResponse> Create(IncomeDetails obj);

        [Delete("/api/Income/Delete/{id}")]
        Task<ApiResponse> Delete(int id);
    }
}

