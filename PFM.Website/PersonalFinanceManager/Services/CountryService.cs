using PersonalFinanceManager.Models.Country;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

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

        public IList<CountryListModel> GetCountries()
        {
            var response = _httpClientExtended.GetList<PFM.Api.Contracts.Country.CountryList>($"/Country/GetList");
            return response.Select(AutoMapper.Mapper.Map<CountryListModel>).ToList();
        }

        public void CreateCountry(CountryEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Country.CountryDetails>(model);
            _httpClientExtended.Post($"/Country/Create", dto);
        }

        public CountryEditModel GetById(int id)
        {
            var response = _httpClientExtended.GetSingle<PFM.Api.Contracts.Country.CountryDetails>($"/Country/Get/{id}");
            return AutoMapper.Mapper.Map<CountryEditModel>(response);
        }

        public void EditCountry(CountryEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Country.CountryDetails>(model);
            _httpClientExtended.Put($"/Country/Edit/{model.Id}", dto);
        }

        public void DeleteCountry(int id)
        {
            _httpClientExtended.Delete($"/Country/Delete/{id}");
        }
    }
}