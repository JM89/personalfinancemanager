using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.AtmWithdraw;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AtmWithdrawController : ControllerBase
    {
        public IAtmWithdrawService _atmWithdrawService { get; set; }

        public AtmWithdrawController(IAtmWithdrawService AtmWithdrawService)
        {
            _atmWithdrawService = AtmWithdrawService;
        }

        [HttpGet("GetList/{accountId}")]
        public async Task<IEnumerable<AtmWithdrawList>> GetList(int accountId)
        {
            return await _atmWithdrawService.GetAtmWithdrawsByAccountId(accountId);
        }

        [HttpGet("Get/{id}")]
        public async Task<AtmWithdrawDetails> Get(int id)
        {
            return await _atmWithdrawService.GetById(id);
        }
        
        [HttpPost("Create")]
        public async Task<bool> Post([FromBody]AtmWithdrawDetails createdObj)
        {
            return await _atmWithdrawService.CreateAtmWithdraw(createdObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _atmWithdrawService.DeleteAtmWithdraw(id);
        }

        [HttpPost("CreateAtmWithdraws")]
        public async Task<bool> CreateAtmWithdraws([FromBody]List<AtmWithdrawDetails> createdObj)
        {
            return await _atmWithdrawService.CreateAtmWithdraws(createdObj);
        }

        [HttpPost("CloseAtmWithdraw/{id}")]
        public async Task<bool> CloseAtmWithdraw(int id)
        {
            return await _atmWithdrawService.CloseAtmWithdraw(id);
        }

        [HttpPost("ChangeDebitStatus/{id}")]
        public async Task<bool> ChangeDebitStatus(int id, bool debitStatus)
        {
            return await _atmWithdrawService.ChangeDebitStatus(id, debitStatus);
        }
    }
}
