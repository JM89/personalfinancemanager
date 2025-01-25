using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.AtmWithdraw;
using PFM.Services;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AtmWithdrawController(IAtmWithdrawService api) : ControllerBase
    {
        [HttpGet("GetList/{accountId}")]
        public async Task<IEnumerable<AtmWithdrawList>> GetList(int accountId)
        {
            return await api.GetAtmWithdrawsByAccountId(accountId);
        }

        [HttpGet("Get/{id}")]
        public async Task<AtmWithdrawDetails> Get(int id)
        {
            return await api.GetById(id);
        }
        
        [HttpPost("Create")]
        public async Task<bool> Post([FromBody]AtmWithdrawDetails createdObj)
        {
            return await api.CreateAtmWithdraw(createdObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await api.DeleteAtmWithdraw(id);
        }

        [HttpPost("CreateAtmWithdraws")]
        public async Task<bool> CreateAtmWithdraws([FromBody]List<AtmWithdrawDetails> createdObj)
        {
            return await api.CreateAtmWithdraws(createdObj);
        }

        [HttpPost("CloseAtmWithdraw/{id}")]
        public async Task<bool> CloseAtmWithdraw(int id)
        {
            return await api.CloseAtmWithdraw(id);
        }

        [HttpPost("ChangeDebitStatus/{id}")]
        public async Task<bool> ChangeDebitStatus(int id, bool debitStatus)
        {
            return await api.ChangeDebitStatus(id, debitStatus);
        }
    }
}
