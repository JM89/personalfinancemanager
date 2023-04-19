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

        public CountryService(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public IList<CountryListModel> GetCountries()
        {
            IList<CountryListModel> result = null;
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var response = httpClient.GetList<PFM.Api.Contracts.Country.CountryList>($"/Country/GetList");
                result = response.Select(AutoMapper.Mapper.Map<CountryListModel>).ToList();
            }
            return result;
        }

        public void CreateCountry(CountryEditModel model)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Country.CountryDetails>(model);
                httpClient.Post($"/Country/Create", dto);
            }
        }

        public CountryEditModel GetById(int id)
        {
            CountryEditModel result = null;
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var response = httpClient.GetSingle<PFM.Api.Contracts.Country.CountryDetails>($"/Country/Get/{id}");
                result = AutoMapper.Mapper.Map<CountryEditModel>(response);
            }
            return result;
        }

        public void EditCountry(CountryEditModel model)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Country.CountryDetails>(model);
                httpClient.Put($"/Country/Edit/{model.Id}", dto);
            }
        }

        public void DeleteCountry(int id)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                httpClient.Delete($"/Country/Delete/{id}");
            }
        }
    }
}