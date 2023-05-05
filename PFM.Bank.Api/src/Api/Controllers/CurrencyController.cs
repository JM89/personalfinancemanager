using Microsoft.AspNetCore.Mvc;
using PFM.Bank.Api.Contracts.Currency;
using Services.Interfaces;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet("GetList")]
        public async Task<IEnumerable<CurrencyList>> GetList()
        {
            return await _currencyService.GetCurrencies();
        }

        [HttpGet("Get/{id}")]
        public async Task<CurrencyDetails> Get(int id)
        {
            return await _currencyService.GetById(id);
        }
    }
}
