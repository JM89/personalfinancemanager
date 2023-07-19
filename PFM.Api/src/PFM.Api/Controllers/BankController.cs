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

        [HttpGet("GetList")]
        public async Task<ApiResponse> GetList()
        {
            return await _bankApi.GetList();
        }

        [HttpGet("Get/{id}")]
        public async Task<ApiResponse> Get(int id)
        {
            return await _bankApi.Get(id);
        }
        
        [HttpPost("Create")]
        public async Task<ApiResponse> Post([FromBody]BankDetails createdObj)
        {
            return await _bankApi.Create(createdObj);
        }
        
        [HttpPut("Edit/{id}")]
        public async Task<ApiResponse> Put(int id, [FromBody]BankDetails editedObj)
        {
            return await _bankApi.Edit(id, editedObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            return await _bankApi.Delete(id);
        }
    }
}
