using AutoMapper;
using PFM.Bank.Api.Contracts.Country;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Repositories;
using Services.Core;

namespace Services
{
    public interface ICountryService : IBaseService
    {
        Task<List<CountryList>> GetCountries();

        Task<CountryDetails> GetById(int id);
    }
    
    public class CountryService(ICountryRepository repository) : ICountryService
    {
        public Task<List<CountryList>> GetCountries()
        {
            var countries = repository.GetList().ToList();

            var mappedCountries = countries.Select(Mapper.Map<CountryList>).ToList();

            mappedCountries.ForEach(country =>
            {
               country.CanBeDeleted = false;
            });

            return Task.FromResult(mappedCountries);
        }

        public Task<CountryDetails> GetById(int id)
        {
            var country = repository.GetById(id);

            if (country == null)
            {
                return null;
            }

            return Task.FromResult(Mapper.Map<CountryDetails>(country));
        }
    }
}