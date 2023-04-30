using PersonalFinanceManager.Models.Country;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services
{
    public class CountryService : ICountryService
    {
        private readonly Serilog.ILogger _logger;
        private readonly IHttpClientExtended _httpClientExtended;

        public CountryService(Serilog.ILogger logger, IHttpClientExtended httpClientExtended)
        {
            _logger = logger;
            _httpClientExtended = httpClientExtended;
        }

        public async Task<IList<CountryListModel>> GetCountries()
        {
            var response = await _httpClientExtended.GetList<PFM.Api.Contracts.Country.CountryList>($"/Country/GetList");
            return response.Select(AutoMapper.Mapper.Map<CountryListModel>).ToList();
        }
    }
}