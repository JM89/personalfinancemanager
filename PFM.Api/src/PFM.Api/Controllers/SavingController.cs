using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.Saving;
using PFM.Services;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SavingController(ISavingService api) : ControllerBase
    {
        [HttpGet("GetList/{accountId}")]
        public async Task<IEnumerable<SavingList>> GetList(int accountId)
        {
            return await api.GetSavingsByAccountId(accountId);
        }

        [HttpGet("Get/{id}")]
        public async Task<SavingDetails> Get(int id)
        {
            return await api.GetById(id);
        }
        
        [HttpPost("Create")]
        public async Task<bool> Post([FromBody]SavingDetails createdObj)
        {
            return await api.CreateSaving(createdObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await api.DeleteSaving(id);
        }
    }
}
