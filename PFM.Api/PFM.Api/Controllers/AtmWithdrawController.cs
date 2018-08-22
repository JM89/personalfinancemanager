using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PFM.DTOs.AtmWithdraw;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/AtmWithdraw")]
    public class AtmWithdrawController : Controller
    {
        public IAtmWithdrawService AtmWithdrawService { get; set; }

        [HttpGet("GetList/{accountId}")]
        public IEnumerable<AtmWithdrawList> GetList(int accountId)
        {
            return AtmWithdrawService.GetAtmWithdrawsByAccountId(accountId);
        }

        [HttpGet("Get/{id}")]
        public AtmWithdrawDetails Get(int id)
        {
            return AtmWithdrawService.GetById(id);
        }
        
        [HttpPost("Create")]
        public void Post([FromBody]AtmWithdrawDetails createdObj)
        {
            AtmWithdrawService.CreateAtmWithdraw(createdObj);
        }
        
        [HttpPut("Edit/{id}")]
        public void Put(int id, [FromBody]AtmWithdrawDetails editedObj)
        {
            AtmWithdrawService.EditAtmWithdraw(editedObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public void Delete(int id)
        {
            AtmWithdrawService.DeleteAtmWithdraw(id);
        }

        [HttpPost("CreateAtmWithdraws")]
        public void CreateAtmWithdraws([FromBody]List<AtmWithdrawDetails> createdObj)
        {
            AtmWithdrawService.CreateAtmWithdraws(createdObj);
        }

        [HttpPost("CloseAtmWithdraw/{id}")]
        public void CloseAtmWithdraw(int id)
        {
            AtmWithdrawService.CloseAtmWithdraw(id);
        }

        [HttpPost("CloseAtmWithdraw/{id}/debitStatus")]
        public void ChangeDebitStatus(int id, bool debitStatus)
        {
            AtmWithdrawService.ChangeDebitStatus(id, debitStatus);
        }
    }
}
