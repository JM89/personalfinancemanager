using System;
using Refit;
using System.Threading.Tasks;
using PFM.TNP.Api.Contracts.IncomeTaxReport;
using PFM.TNP.Api.Contracts.Shared;

namespace PFM.Services.ExternalServices.TaxAndPensionApi
{
    public interface IIncomeTaxReportApi
    {
        [Get("/api/IncomeTaxReport/GetList/{userId}")]
        Task<ApiResponse> GetList(string userId);

        [Get("/api/IncomeTaxReport/Get/{id}")]
        Task<ApiResponse> Get(Guid id);

        [Post("/api/IncomeTaxReport/Create/{userId}")]
        Task<ApiResponse> Create(string userId, IncomeTaxReportSaveRequest request);

        [Put("/api/IncomeTaxReport/Edit/{id}")]
        Task<ApiResponse> Edit(Guid id, IncomeTaxReportSaveRequest request);

        [Delete("/api/IncomeTaxReport/Delete/{id}")]
        Task<ApiResponse> Delete(Guid id);
    }
}
