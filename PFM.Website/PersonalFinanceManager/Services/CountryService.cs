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

        public async Task<bool> CreateCountry(CountryEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Country.CountryDetails>(model);
            return await _httpClientExtended.Post($"/Country/Create", dto);
        }

        public async Task<CountryEditModel> GetById(int id)
        {
            var response = await _httpClientExtended.GetSingle<PFM.Api.Contracts.Country.CountryDetails>($"/Country/Get/{id}");
            return AutoMapper.Mapper.Map<CountryEditModel>(response);
        }

        public async Task<bool> EditCountry(CountryEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Country.CountryDetails>(model);
            return await _httpClientExtended.Put($"/Country/Edit/{model.Id}", dto);
        }

        public async Task<bool> DeleteCountry(int id)
        {
            return await _httpClientExtended.Delete($"/Country/Delete/{id}");
        }
    }
}