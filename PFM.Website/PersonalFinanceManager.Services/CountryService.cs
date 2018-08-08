using System;
using System.Collections.Generic;
using PersonalFinanceManager.Models.Country;
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Services
{
    public class CountryService : ICountryService
    {
        public IList<CountryListModel> GetCountries()
        {
            throw new NotImplementedException();
        }

        public void CreateCountry(CountryEditModel countryEditModel)
        {
            throw new NotImplementedException();
        }

        public CountryEditModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void EditCountry(CountryEditModel countryEditModel)
        {
            throw new NotImplementedException();
        }

        public void DeleteCountry(int id)
        {
            throw new NotImplementedException();
        }
    }
}