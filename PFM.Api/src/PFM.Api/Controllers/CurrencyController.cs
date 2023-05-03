using Api.Contracts.Shared;
using Microsoft.AspNetCore.Mvc;
using PFM.Services.ExternalServices.BankApi;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyApi _currencyApi;

        public CurrencyController(ICurrencyApi currencyApi)
        {
            _currencyApi = currencyApi;
        }

        [HttpGet("GetList")]
        public async Task<ApiResponse> GetList()
        {
            return await _currencyApi.GetList();
        }

        [HttpGet("Get/{id}")]
        public async Task<ApiResponse> Get(int id)
        {
            return await _currencyApi.Get(id);
        }
    }
}
