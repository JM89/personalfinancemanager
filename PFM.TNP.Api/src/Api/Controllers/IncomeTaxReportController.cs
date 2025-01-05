using Microsoft.AspNetCore.Mvc;
using PFM.TNP.Api.Contracts.IncomeTaxReport;
using PFM.TNP.Api.Contracts.Pension;
using Services;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IncomeTaxReportController(IIncomeTaxReportService incomeTaxReportService) : ControllerBase
{
    [HttpGet("GetList/{userId}")]
    public async Task<IEnumerable<IncomeTaxReportList>> GetList(string userId)
    {
        return await incomeTaxReportService.GetList(userId);
    }

    [HttpGet("Get/{id}")]
    public async Task<IncomeTaxReportDetails> Get(Guid id)
    {
        return await incomeTaxReportService.GetById(id);
    }
        
    [HttpPost("Create/{userId}")]
    public async Task<bool> Post(string userId, [FromBody]IncomeTaxReportSaveRequest createdObj)
    {
        return await incomeTaxReportService.Create(createdObj, userId);
    }
        
    [HttpPut("Edit/{id}")]
    public async Task<bool> Put(Guid id, [FromBody]IncomeTaxReportSaveRequest editedObj)
    {
        return await incomeTaxReportService.Edit(id, editedObj);
    }
        
    [HttpDelete("Delete/{id}")]
    public async Task<bool> Delete(Guid id)
    {
        return await incomeTaxReportService.Delete(id);
    }
}