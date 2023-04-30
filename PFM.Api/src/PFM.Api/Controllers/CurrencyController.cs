using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.Currency;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
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
        public IEnumerable<CurrencyList> GetList()
        {
            return _currencyService.GetCurrencies();
        }

        [HttpGet("Get/{id}")]
        public CurrencyDetails Get(int id)
        {
            return _currencyService.GetById(id);
        }
    }
}
