using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PFM.DTOs.Country;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Country")]
    public class CountryController : Controller
    {
        private readonly ICountryService _CountryService;

        public CountryController(ICountryService CountryService)
        {
            _CountryService = CountryService;
        }

        [HttpGet("GetList")]
        public IEnumerable<CountryList> GetList()
        {
            return _CountryService.GetCountries();
        }

        [HttpGet("Get/{id}")]
        public CountryDetails Get(int id)
        {
            return _CountryService.GetById(id);
        }
        
        [HttpPost("Create")]
        public void Post([FromBody]CountryDetails createdObj)
        {
            _CountryService.CreateCountry(createdObj);
        }
        
        [HttpPut("Edit/{id}")]
        public void Put(int id, [FromBody]CountryDetails editedObj)
        {
            _CountryService.EditCountry(editedObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public void Delete(int id)
        {
            _CountryService.DeleteCountry(id);
        }
    }
}
