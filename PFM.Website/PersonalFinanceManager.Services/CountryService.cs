using System;
using System.Collections.Generic;
using System.Linq;
using PersonalFinanceManager.Models.Country;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Services
{
    public class CountryService : ICountryService
    {
        public IList<CountryListModel> GetCountries()
        {
            IList<CountryListModel> result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetList<PFM.DTOs.Country.CountryList>($"/Country/GetList");
                result = response.Select(AutoMapper.Mapper.Map<CountryListModel>).ToList();
            }
            return result;
        }

        public void CreateCountry(CountryEditModel model)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PFM.DTOs.Country.CountryDetails>(model);
                httpClient.Post($"/Country/Create", dto);
            }
        }

        public CountryEditModel GetById(int id)
        {
            CountryEditModel result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetSingle<PFM.DTOs.Country.CountryDetails>($"/Country/Get/{id}");
                result = AutoMapper.Mapper.Map<CountryEditModel>(response);
            }
            return result;
        }

        public void EditCountry(CountryEditModel model)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PFM.DTOs.Country.CountryDetails>(model);
                httpClient.Put($"/Country/Edit/{model.Id}", dto);
            }
        }

        public void DeleteCountry(int id)
        {
            using (var httpClient = new HttpClientExtended())
            {
                httpClient.Delete($"/Country/Delete/{id}");
            }
        }
    }
}