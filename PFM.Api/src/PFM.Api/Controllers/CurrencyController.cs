using Api.Contracts.Shared;
using Microsoft.AspNetCore.Mvc;
using PFM.Services.ExternalServices.BankApi;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController(ICurrencyApi api) : ControllerBase
    {
        [HttpGet("GetList")]
        public async Task<ApiResponse> GetList()
        {
            return await api.GetList();
        }

        [HttpGet("Get/{id}")]
        public async Task<ApiResponse> Get(int id)
        {
            return await api.Get(id);
        }
    }
}
