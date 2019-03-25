using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PFM.Services.Interfaces;
using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.DTOs.Country;

namespace PFM.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly ISalaryRepository _salaryRepository;
        private readonly IPensionRepository _pensionRepository;
        private readonly ITaxRepository _taxRepository;
        private readonly IBankRepository _bankRepository;

        public CountryService(ICountryRepository countryRepository, IPensionRepository pensionRepository, ITaxRepository taxRepository, ISalaryRepository salaryRepository, IBankRepository bankRepository)
        {
            this._countryRepository = countryRepository;
            this._bankRepository = bankRepository;
            this._salaryRepository = salaryRepository;
            this._pensionRepository = pensionRepository;
            this._taxRepository = taxRepository;
        }

        public IList<CountryList> GetCountries()
        {
            var countries = _countryRepository.GetList().ToList();

            var mappedCountries = countries.Select(Mapper.Map<CountryList>).ToList();

            mappedCountries.ForEach(country =>
            {
                var hasBank = _bankRepository.GetList().Any(x => x.CountryId == country.Id);
                var hasSalary = _salaryRepository.GetList().Any(x => x.CountryId == country.Id);
                var hasPension = _pensionRepository.GetList().Any(x => x.CountryId == country.Id);
                var hasTax = _taxRepository.GetList().Any(x => x.CountryId == country.Id);
                
                country.CanBeDeleted = !hasBank && !hasSalary && !hasPension && !hasTax;
            });

            return mappedCountries;
        }

        public void CreateCountry(CountryDetails countryDetails)
        {
            var country = Mapper.Map<Country>(countryDetails);
            _countryRepository.Create(country);
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

        public void EditCountry(CountryDetails countryDetails)
        {
            var country = _countryRepository.GetById(countryDetails.Id);
            country.Name = countryDetails.Name;
            _countryRepository.Update(country);
        }

        public void DeleteCountry(int id)
        {
            var country = _countryRepository.GetById(id);
            _countryRepository.Delete(country);
        }
    }
}