using PersonalFinanceManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Models.Country;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using PersonalFinanceManager.DataAccess;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;

namespace PersonalFinanceManager.Services
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

        public IList<CountryListModel> GetCountries()
        {
            var countries = _countryRepository.GetList().ToList();

            var countriesModel = countries.Select(Mapper.Map<CountryListModel>).ToList();

            countriesModel.ForEach(country =>
            {
                var hasBank = _bankRepository.GetList().Any(x => x.CountryId == country.Id);
                var hasSalary = _salaryRepository.GetList().Any(x => x.CountryId == country.Id);
                var hasPension = _pensionRepository.GetList().Any(x => x.CountryId == country.Id);
                var hasTax = _taxRepository.GetList().Any(x => x.CountryId == country.Id);
                
                country.CanBeDeleted = !hasBank && !hasSalary && !hasPension && !hasTax;
            });

            return countriesModel;
        }

        public void CreateCountry(CountryEditModel countryEditModel)
        {
            var countryModel = Mapper.Map<CountryModel>(countryEditModel);
            _countryRepository.Create(countryModel);
        }

        public CountryEditModel GetById(int id)
        {
            var country = _countryRepository.GetById(id);

            if (country == null)
            {
                return null;
            }

            return Mapper.Map<CountryEditModel>(country);
        }

        public void EditCountry(CountryEditModel countryEditModel)
        {
            var countryModel = _countryRepository.GetById(countryEditModel.Id);
            countryModel.Name = countryEditModel.Name;
            _countryRepository.Update(countryModel);
        }

        public void DeleteCountry(int id)
        {
            var countryModel = _countryRepository.GetById(id);
            _countryRepository.Delete(countryModel);
        }
    }
}