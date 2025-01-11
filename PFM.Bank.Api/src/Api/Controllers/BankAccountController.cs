using Microsoft.AspNetCore.Mvc;
using PFM.Bank.Api.Contracts.Account;
using Services;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankAccountController(IBankAccountService service) : ControllerBase
    {
        [HttpGet("GetList/{userId}")]
        public async Task<IEnumerable<AccountList>> GetList(string userId)
        {
            return await service.GetAccountsByUser(userId);
        }

        [HttpGet("Get/{id}")]
        public async Task<AccountDetails> Get(int id)
        {
            return await service.GetById(id);
        }
        
        [HttpPost("Create/{userId}")]
        public async Task<bool> Post(string userId, [FromBody]AccountDetails createdObj)
        {
            return await service.CreateBankAccount(createdObj, userId);
        }
        
        [HttpPut("Edit/{id}/{userId}")]
        public async Task<bool> Put(int id, string userId, [FromBody]AccountDetails editedObj)
        {
            editedObj.Id = id;
            return await service.EditBankAccount(editedObj, userId);
        }
        
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await service.DeleteBankAccount(id);
        }

        [HttpPost("SetAsFavorite/{id}")]
        public async Task<bool> SetAsFavorite(int id)
        {
            return await service.SetAsFavorite(id);
        }
    }
}
