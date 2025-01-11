using Microsoft.AspNetCore.Mvc;
using PFM.Services.ExternalServices.TaxAndPensionApi;
using PFM.TNP.Api.Contracts.IncomeTaxReport;
using PFM.TNP.Api.Contracts.Shared;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncomeTaxReportController(IIncomeTaxReportApi api) : ControllerBase
    {
        [HttpGet("GetList/{userId}")]
        public async Task<ApiResponse> GetList(string userId)
        {
            return await api.GetList(userId);
        }

        [HttpGet("Get/{id}")]
        public async Task<ApiResponse> Get(Guid id)
        {
            return await api.Get(id);
        }
        
        [HttpPost("Create/{userId}")]
        public async Task<ApiResponse> Post(string userId, [FromBody]IncomeTaxReportSaveRequest request)
        {
            return await api.Create(userId, request);
        }
        
        [HttpPut("Edit/{id}")]
        public async Task<ApiResponse> Put(Guid id, [FromBody]IncomeTaxReportSaveRequest request)
        {
            return await api.Edit(id, request);
        }
        
        [HttpDelete("Delete/{id}")]
        public async Task<ApiResponse> Delete(Guid id)
        {
            return await api.Delete(id);
        }
    }
}
