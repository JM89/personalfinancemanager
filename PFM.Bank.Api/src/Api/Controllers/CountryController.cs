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
        public IEnumerable<CountryList> GetList()
        {
            return _countryService.GetCountries();
        }

        [HttpGet("Get/{id}")]
        public CountryDetails Get(int id)
        {
            return _countryService.GetById(id);
        }
    }
}
