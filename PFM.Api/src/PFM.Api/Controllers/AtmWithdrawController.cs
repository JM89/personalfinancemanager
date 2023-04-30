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
        public IEnumerable<AtmWithdrawList> GetList(int accountId)
        {
            return _atmWithdrawService.GetAtmWithdrawsByAccountId(accountId);
        }

        [HttpGet("Get/{id}")]
        public AtmWithdrawDetails Get(int id)
        {
            return _atmWithdrawService.GetById(id);
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
        public void CloseAtmWithdraw(int id)
        {
            _atmWithdrawService.CloseAtmWithdraw(id);
        }

        [HttpPost("CloseAtmWithdraw/{id}/debitStatus")]
        public void ChangeDebitStatus(int id, bool debitStatus)
        {
            _atmWithdrawService.ChangeDebitStatus(id, debitStatus);
        }
    }
}
