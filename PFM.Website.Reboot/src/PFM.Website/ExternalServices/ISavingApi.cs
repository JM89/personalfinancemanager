using Api.Contracts.Shared;
using PFM.Api.Contracts.Saving;
using Refit;

namespace PFM.Website.ExternalServices
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

