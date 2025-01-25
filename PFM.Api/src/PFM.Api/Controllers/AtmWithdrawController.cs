using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.AtmWithdraw;
using PFM.Services;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AtmWithdrawController(IAtmWithdrawService service) : ControllerBase
    {
        public IAtmWithdrawService _service { get; set; } = service;

        [HttpGet("GetList/{accountId}")]
        public async Task<IEnumerable<AtmWithdrawList>> GetList(int accountId)
        {
            return await _service.GetAtmWithdrawsByAccountId(accountId);
        }

        [HttpGet("Get/{id}")]
        public async Task<AtmWithdrawDetails> Get(int id)
        {
            return await _service.GetById(id);
        }
        
        [HttpPost("Create")]
        public async Task<bool> Post([FromBody]AtmWithdrawDetails createdObj)
        {
            return await _service.CreateAtmWithdraw(createdObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _service.DeleteAtmWithdraw(id);
        }

        [HttpPost("CreateAtmWithdraws")]
        public async Task<bool> CreateAtmWithdraws([FromBody]List<AtmWithdrawDetails> createdObj)
        {
            return await _service.CreateAtmWithdraws(createdObj);
        }

        [HttpPost("CloseAtmWithdraw/{id}")]
        public async Task<bool> CloseAtmWithdraw(int id)
        {
            return await _service.CloseAtmWithdraw(id);
        }

        [HttpPost("ChangeDebitStatus/{id}")]
        public async Task<bool> ChangeDebitStatus(int id, bool debitStatus)
        {
            return await _service.ChangeDebitStatus(id, debitStatus);
        }
    }
}
