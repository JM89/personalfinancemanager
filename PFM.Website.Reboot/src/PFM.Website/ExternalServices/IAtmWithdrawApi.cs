using PFM.Api.Contracts.Shared;
using PFM.Api.Contracts.AtmWithdraw;
using Refit;

namespace PFM.Website.ExternalServices
{
	public interface IAtmWithdrawApi
	{
        [Get("/api/AtmWithdraw/GetList/{accountId}")]
        Task<ApiResponse> GetList(int accountId);

        [Get("/api/AtmWithdraw/Get/{id}")]
        Task<ApiResponse> Get(int id);

        [Post("/api/AtmWithdraw/Create/")]
        Task<ApiResponse> Create(AtmWithdrawDetails obj);

        [Delete("/api/AtmWithdraw/Delete/{id}")]
        Task<ApiResponse> Delete(int id);

        [Post("/api/AtmWithdraw/CloseAtmWithdraw/{id}")]
        Task<ApiResponse> CloseAtmWithdraw(int id);

        [Post("/api/AtmWithdraw/ChangeDebitStatus/{id}")]
        Task<ApiResponse> ChangeDebitStatus(int id, bool debitStatus);
    }
}

