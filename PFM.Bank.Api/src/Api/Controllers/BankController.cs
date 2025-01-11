using Microsoft.AspNetCore.Mvc;
using PFM.Bank.Api.Contracts.Bank;
using Services;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankController(IBankService service) : ControllerBase
    {
        [HttpGet("GetList/{userId}")]
        public async Task<IEnumerable<BankList>> GetList(string userId)
        {
            return await service.GetBanks(userId);
        }

        [HttpGet("Get/{id}")]
        public async Task<BankDetails> Get(int id)
        {
            return await service.GetById(id);
        }
        
        [HttpPost("Create/{userId}")]
        public async Task<bool> Post(string userId, [FromBody]BankDetails createdObj)
        {
            return await service.CreateBank(createdObj, userId);
        }
        
        [HttpPut("Edit/{id}/{userId}")]
        public async Task<bool> Put(int id, string userId, [FromBody]BankDetails editedObj)
        {
            editedObj.Id = id;
            return await service.EditBank(editedObj, userId);
        }
        
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await service.DeleteBank(id);
        }
    }
}
