using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.Saving;
using PFM.Services;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SavingController(ISavingService service) : ControllerBase
    {
        [HttpGet("GetList/{accountId}")]
        public async Task<IEnumerable<SavingList>> GetList(int accountId)
        {
            return await service.GetSavingsByAccountId(accountId);
        }

        [HttpGet("Get/{id}")]
        public async Task<SavingDetails> Get(int id)
        {
            return await service.GetById(id);
        }
        
        [HttpPost("Create")]
        public async Task<bool> Post([FromBody]SavingDetails createdObj)
        {
            return await service.CreateSaving(createdObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await service.DeleteSaving(id);
        }
    }
}
