using Api.Contracts.Shared;
using Microsoft.AspNetCore.Mvc;
using PFM.Bank.Api.Contracts.Bank;
using PFM.Services.ExternalServices.BankApi;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankController : ControllerBase
    {
        private readonly IBankApi _bankApi;

        public BankController(IBankApi bankApi)
        {
            _bankApi = bankApi;
        }

        [HttpGet("GetList/{userId}")]
        public async Task<ApiResponse> GetList(string userId)
        {
            return await _bankApi.GetList(userId);
        }

        [HttpGet("Get/{id}")]
        public async Task<ApiResponse> Get(int id)
        {
            return await _bankApi.Get(id);
        }
        
        [HttpPost("Create/{userId}")]
        public async Task<ApiResponse> Post(string userId, [FromBody]BankDetails createdObj)
        {
            return await _bankApi.Create(userId, createdObj);
        }
        
        [HttpPut("Edit/{id}/{userId}")]
        public async Task<ApiResponse> Put(int id, string userId, [FromBody]BankDetails editedObj)
        {
            return await _bankApi.Edit(id, userId, editedObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            return await _bankApi.Delete(id);
        }
    }
}
