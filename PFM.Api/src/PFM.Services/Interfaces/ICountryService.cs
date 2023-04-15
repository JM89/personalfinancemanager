﻿using System.Collections.Generic;
using PFM.Api.Contracts.Country;

namespace PFM.Services.Interfaces
{
    public interface ICountryService : IBaseService
    {
        IList<CountryList> GetCountries();

        void CreateCountry(CountryDetails countryDetails);

        CountryDetails GetById(int id);

        void EditCountry(CountryDetails countryDetails);

        void DeleteCountry(int id);
    }
}