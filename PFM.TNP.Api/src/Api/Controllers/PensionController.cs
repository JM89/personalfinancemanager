using Microsoft.AspNetCore.Mvc;
using PFM.Pension.Api.Contracts.Pension;
using Services;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PensionController(IPensionService pensionService) : ControllerBase
{
    [HttpGet("GetList/{userId}")]
    public async Task<IEnumerable<PensionList>> GetList(string userId)
    {
        return await pensionService.GetList(userId);
    }

    [HttpGet("Get/{id}")]
    public async Task<PensionDetails> Get(Guid id)
    {
        return await pensionService.GetById(id);
    }
        
    [HttpPost("Create/{userId}")]
    public async Task<bool> Post(string userId, [FromBody]PensionCreateRequest createdObj)
    {
        return await pensionService.Create(createdObj, userId);
    }
        
    [HttpPut("Edit/{id}/{userId}")]
    public async Task<bool> Put(Guid id, string userId, [FromBody]PensionDetails editedObj)
    {
        editedObj.Id = id;
        return await pensionService.Edit(editedObj, userId);
    }
        
    [HttpDelete("Delete/{id}")]
    public async Task<bool> Delete(Guid id)
    {
        return await pensionService.Delete(id);
    }
}