using Microsoft.AspNetCore.Mvc;
using PFM.Bank.Api.Contracts.Account;
using Services.Interfaces;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService _bankAccountService;

        public BankAccountController(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }

        [HttpGet("GetList/{userId}")]
        public IEnumerable<AccountList> GetList(string userId)
        {
            return _bankAccountService.GetAccountsByUser(userId);
        }

        [HttpGet("Get/{id}")]
        public AccountDetails Get(int id)
        {
            return _bankAccountService.GetById(id);
        }
        
        [HttpPost("Create/{userId}")]
        public async Task<bool> Post(string userId, [FromBody]AccountDetails createdObj)
        {
            return await _bankAccountService.CreateBankAccount(createdObj, userId);
        }
        
        [HttpPut("Edit/{id}/{userId}")]
        public async Task<bool> Put(int id, string userId, [FromBody]AccountDetails editedObj)
        {
            editedObj.Id = id;
            return await _bankAccountService.EditBankAccount(editedObj, userId);
        }
        
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _bankAccountService.DeleteBankAccount(id);
        }

        [HttpPost("SetAsFavorite/{id}")]
        public bool SetAsFavorite(int id)
        {
            _bankAccountService.SetAsFavorite(id);
            return true;
        }
    }
}
