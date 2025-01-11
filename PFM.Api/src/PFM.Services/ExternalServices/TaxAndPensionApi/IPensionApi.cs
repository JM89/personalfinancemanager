using System;
using Refit;
using System.Threading.Tasks;
using PFM.TNP.Api.Contracts.Pension;
using PFM.TNP.Api.Contracts.Shared;

namespace PFM.Services.ExternalServices.TaxAndPensionApi
{
    public interface IPensionApi
    {
        [Get("/api/Pension/GetList/{userId}")]
        Task<ApiResponse> GetList(string userId);

        [Get("/api/Pension/Get/{id}")]
        Task<ApiResponse> Get(Guid id);

        [Post("/api/Pension/Create/{userId}")]
        Task<ApiResponse> Create(string userId, PensionSaveRequest request);

        [Put("/api/Pension/Edit/{id}")]
        Task<ApiResponse> Edit(Guid id, PensionSaveRequest request);

        [Delete("/api/Pension/Delete/{id}")]
        Task<ApiResponse> Delete(Guid id);
    }
}
