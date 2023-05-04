using Api.Contracts.Shared;
using Microsoft.AspNetCore.Mvc;
using PFM.Bank.Api.Contracts.Account;
using PFM.Services.ExternalServices.BankApi;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountApi _bankAccountApi;

        public BankAccountController(IBankAccountApi bankAccountApi)
        {
            _bankAccountApi = bankAccountApi;
        }

        [HttpGet("GetList/{userId}")]
        public async Task<ApiResponse> GetList(string userId)
        {
            return await _bankAccountApi.GetList(userId);
        }

        [HttpGet("Get/{id}")]
        public async Task<ApiResponse> Get(int id)
        {
            return await _bankAccountApi.Get(id);
        }
        
        [HttpPost("Create/{userId}")]
        public async Task<ApiResponse> Post(string userId, [FromBody]AccountDetails createdObj)
        {
            return await _bankAccountApi.Create(userId, createdObj);
        }
        
        [HttpPut("Edit/{id}/{userId}")]
        public async Task<ApiResponse> Put(int id, string userId, [FromBody]AccountDetails editedObj)
        {
            return await _bankAccountApi.Edit(id, userId, editedObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            return await _bankAccountApi.Delete(id);
        }

        [HttpPost("SetAsFavorite/{id}")]
        public async Task<ApiResponse> SetAsFavorite(int id)
        {
            return await _bankAccountApi.SetAsFavorite(id);
        }
    }
}
