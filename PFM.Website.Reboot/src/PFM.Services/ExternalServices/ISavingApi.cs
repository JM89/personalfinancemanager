using PFM.Api.Contracts.Saving;
using PFM.Api.Contracts.Shared;
using Refit;

namespace PFM.Services.ExternalServices
{
	public interface ISavingApi
	{
        [Get("/api/Saving/GetList/{accountId}")]
        Task<ApiResponse> GetList(int accountId);

        [Get("/api/Saving/Get/{id}")]
        Task<ApiResponse> Get(int id);

        [Post("/api/Saving/Create/")]
        Task<ApiResponse> Create(SavingDetails obj);

        [Delete("/api/Saving/Delete/{id}")]
        Task<ApiResponse> Delete(int id);
    }
}

