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
        public IEnumerable<SavingList> GetList(int accountId)
        {
            return _savingService.GetSavingsByAccountId(accountId);
        }

        [HttpGet("Get/{id}")]
        public SavingDetails Get(int id)
        {
            return _savingService.GetById(id);
        }
        
        [HttpPost("Create")]
        public async Task<bool> Post([FromBody]SavingDetails createdObj)
        {
            return await _savingService.CreateSaving(createdObj);
        }
        
        [HttpPut("Edit/{id}")]
        public async Task<bool> Put(int id, [FromBody]SavingDetails editedObj)
        {
            return await _savingService.EditSaving(editedObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _savingService.DeleteSaving(id);
        }
    }
}
