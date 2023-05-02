using AutoMapper;
using PFM.Bank.Api.Contracts.Country;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PFM.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;

        public CountryService(ICountryRepository countryRepository)
        {
            this._countryRepository = countryRepository;
        }

        public IList<CountryList> GetCountries()
        {
            var countries = _countryRepository.GetList().ToList();

            var mappedCountries = countries.Select(Mapper.Map<CountryList>).ToList();

            mappedCountries.ForEach(country =>
            {
               country.CanBeDeleted = false;
            });

            return mappedCountries;
        }

        public CountryDetails GetById(int id)
        {
            var country = _countryRepository.GetById(id);

            if (country == null)
            {
                return null;
            }

            return Mapper.Map<CountryDetails>(country);
        }
    }
}