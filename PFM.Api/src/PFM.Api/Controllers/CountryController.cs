using Api.Contracts.Shared;
using Microsoft.AspNetCore.Mvc;
using PFM.Services.ExternalServices.BankApi;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryApi _countryApi;

        public CountryController(ICountryApi countryApi)
        {
            _countryApi = countryApi;
        }

        [HttpGet("GetList")]
        public async Task<ApiResponse> GetList()
        {
            return await _countryApi.GetList();
        }

        [HttpGet("Get/{id}")]
        public async Task<ApiResponse> Get(int id)
        {
            return await _countryApi.Get(id);
        }
    }
}
