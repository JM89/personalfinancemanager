using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.Account;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService _BankAccountService;

        public BankAccountController(IBankAccountService BankAccountService)
        {
            _BankAccountService = BankAccountService;
        }

        [HttpGet("GetList/{userId}")]
        public IEnumerable<AccountList> GetList(string userId)
        {
            return _BankAccountService.GetAccountsByUser(userId);
        }

        [HttpGet("Get/{id}")]
        public AccountDetails Get(int id)
        {
            return _BankAccountService.GetById(id);
        }
        
        [HttpPost("Create/{userId}")]
        public async Task<bool> Post(string userId, [FromBody]AccountDetails createdObj)
        {
            return await _BankAccountService.CreateBankAccount(createdObj, userId);
        }
        
        [HttpPut("Edit/{id}/{userId}")]
        public void Put(int id, string userId, [FromBody]AccountDetails editedObj)
        {
            _BankAccountService.EditBankAccount(editedObj, userId);
        }
        
        [HttpDelete("Delete/{id}")]
        public void Delete(int id)
        {
            _BankAccountService.DeleteBankAccount(id);
        }

        [HttpPost("SetAsFavorite/{id}")]
        public void SetAsFavorite(int id)
        {
            _BankAccountService.SetAsFavorite(id);
        }
    }
}
