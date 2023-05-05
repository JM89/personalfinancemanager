using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.Saving;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SavingController : ControllerBase
    {
        private readonly ISavingService _savingService;

        public SavingController(ISavingService SavingService)
        {
            _savingService = SavingService;
        }

        [HttpGet("GetList/{accountId}")]
        public async Task<IEnumerable<SavingList>> GetList(int accountId)
        {
            return await _savingService.GetSavingsByAccountId(accountId);
        }

        [HttpGet("Get/{id}")]
        public async Task<SavingDetails> Get(int id)
        {
            return await _savingService.GetById(id);
        }
        
        [HttpPost("Create")]
        public async Task<bool> Post([FromBody]SavingDetails createdObj)
        {
            return await _savingService.CreateSaving(createdObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _savingService.DeleteSaving(id);
        }
    }
}
