using Microsoft.AspNetCore.Mvc;
using PFM.Bank.Api.Contracts.Bank;
using Services.Interfaces;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankController : ControllerBase
    {
        private readonly IBankService _bankService;

        public BankController(IBankService bankService)
        {
            _bankService = bankService;
        }

        [HttpGet("GetList/{userId}")]
        public async Task<IEnumerable<BankList>> GetList(string userId)
        {
            return await _bankService.GetBanks(userId);
        }

        [HttpGet("Get/{id}")]
        public async Task<BankDetails> Get(int id)
        {
            return await _bankService.GetById(id);
        }
        
        [HttpPost("Create/{userId}")]
        public async Task<bool> Post(string userId, [FromBody]BankDetails createdObj)
        {
            return await _bankService.CreateBank(createdObj, userId);
        }
        
        [HttpPut("Edit/{id}/{userId}")]
        public async Task<bool> Put(int id, string userId, [FromBody]BankDetails editedObj)
        {
            editedObj.Id = id;
            return await _bankService.EditBank(editedObj, userId);
        }
        
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _bankService.DeleteBank(id);
        }
    }
}
