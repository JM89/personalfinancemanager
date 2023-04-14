using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PFM.Services.DTOs.AtmWithdraw;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/AtmWithdraw")]
    public class AtmWithdrawController : Controller
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
        public void Post([FromBody]AtmWithdrawDetails createdObj)
        {
            _atmWithdrawService.CreateAtmWithdraw(createdObj);
        }
        
        [HttpPut("Edit/{id}")]
        public void Put(int id, [FromBody]AtmWithdrawDetails editedObj)
        {
            _atmWithdrawService.EditAtmWithdraw(editedObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public void Delete(int id)
        {
            _atmWithdrawService.DeleteAtmWithdraw(id);
        }

        [HttpPost("CreateAtmWithdraws")]
        public void CreateAtmWithdraws([FromBody]List<AtmWithdrawDetails> createdObj)
        {
            _atmWithdrawService.CreateAtmWithdraws(createdObj);
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
