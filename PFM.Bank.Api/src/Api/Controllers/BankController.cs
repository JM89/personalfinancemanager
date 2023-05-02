using Microsoft.AspNetCore.Mvc;
using PFM.Bank.Api.Contracts.Bank;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
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
        public void Post([FromBody]BankDetails createdObj)
        {
            _bankService.CreateBank(createdObj);
        }
        
        [HttpPut("Edit/{id}")]
        public void Put(int id, [FromBody]BankDetails editedObj)
        {
            _bankService.EditBank(editedObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public void Delete(int id)
        {
            _bankService.DeleteBank(id);
        }
    }
}
