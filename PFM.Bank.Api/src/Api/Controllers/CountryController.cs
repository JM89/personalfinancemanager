using Microsoft.AspNetCore.Mvc;
using PFM.Bank.Api.Contracts.Country;
using Services;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController(ICountryService service) : ControllerBase
    {
        [HttpGet("GetList")]
        public async Task<IEnumerable<CountryList>> GetList()
        {
            return await service.GetCountries();
        }

        [HttpGet("Get/{id}")]
        public async Task<CountryDetails> Get(int id)
        {
            return await service.GetById(id);
        }
    }
}
