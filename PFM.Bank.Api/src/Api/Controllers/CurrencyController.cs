using Microsoft.AspNetCore.Mvc;
using PFM.Bank.Api.Contracts.Currency;
using Services;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController(ICurrencyService service) : ControllerBase
    {
        [HttpGet("GetList")]
        public async Task<IEnumerable<CurrencyList>> GetList()
        {
            return await service.GetCurrencies();
        }

        [HttpGet("Get/{id}")]
        public async Task<CurrencyDetails> Get(int id)
        {
            return await service.GetById(id);
        }
    }
}
