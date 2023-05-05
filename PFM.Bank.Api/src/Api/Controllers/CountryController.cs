using Microsoft.AspNetCore.Mvc;
using PFM.Bank.Api.Contracts.Country;
using Services.Interfaces;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService CountryService)
        {
            _countryService = CountryService;
        }

        [HttpGet("GetList")]
        public async Task<IEnumerable<CountryList>> GetList()
        {
            return await _countryService.GetCountries();
        }

        [HttpGet("Get/{id}")]
        public async Task<CountryDetails> Get(int id)
        {
            return await _countryService.GetById(id);
        }
    }
}
