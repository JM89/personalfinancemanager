using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PFM.Services.DTOs.Currency;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Currency")]
    public class CurrencyController : Controller
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
        
        [HttpPost("Create")]
        public void Post([FromBody]CurrencyDetails createdObj)
        {
            _currencyService.CreateCurrency(createdObj);
        }
        
        [HttpPut("Edit/{id}")]
        public void Put(int id, [FromBody]CurrencyDetails editedObj)
        {
            _currencyService.EditCurrency(editedObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public void Delete(int id)
        {
            _currencyService.DeleteCurrency(id);
        }
    }
}
