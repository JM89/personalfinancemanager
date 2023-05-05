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

        [HttpGet("GetList")]
        public async Task<IEnumerable<BankList>> GetList()
        {
            return await _bankService.GetBanks();
        }

        [HttpGet("Get/{id}")]
        public async Task<BankDetails> Get(int id)
        {
            return await _bankService.GetById(id);
        }
        
        [HttpPost("Create")]
        public async Task<bool> Post([FromBody]BankDetails createdObj)
        {
            return await _bankService.CreateBank(createdObj);
        }
        
        [HttpPut("Edit/{id}")]
        public async Task<bool> Put(int id, [FromBody]BankDetails editedObj)
        {
            editedObj.Id = id;
            return await _bankService.EditBank(editedObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _bankService.DeleteBank(id);
        }
    }
}
