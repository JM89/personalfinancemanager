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
        public IEnumerable<BankList> GetList()
        {
            return _bankService.GetBanks();
        }

        [HttpGet("Get/{id}")]
        public BankDetails Get(int id)
        {
            return _bankService.GetById(id);
        }
        
        [HttpPost("Create")]
        public bool Post([FromBody]BankDetails createdObj)
        {
            _bankService.CreateBank(createdObj);
            return true;
        }
        
        [HttpPut("Edit/{id}")]
        public bool Put(int id, [FromBody]BankDetails editedObj)
        {
            editedObj.Id = id;
            _bankService.EditBank(editedObj);
            return true;
        }
        
        [HttpDelete("Delete/{id}")]
        public bool Delete(int id)
        {
            _bankService.DeleteBank(id);
            return true;
        }
    }
}
