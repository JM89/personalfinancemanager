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

namespace PersonalFinanceManager.Services
{
    public class CountryService : ICountryService
    {
        private ApplicationDbContext _db;

        public CountryService(ApplicationDbContext db)
        {
            this._db = db;
        }

        public IList<CountryListModel> GetCountries()
        {
            var countries = _db.CountryModels.ToList();

            var countriesModel = countries.Select(x => Mapper.Map<CountryListModel>(x)).ToList();

            countriesModel.ForEach(country =>
            {
                country.CanBeDeleted = !_db.BankModels.Any(x => x.CountryId == country.Id);
            });

            return countriesModel;
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void CreateCountry(CountryEditModel countryEditModel)
        {
            var countryModel = Mapper.Map<CountryModel>(countryEditModel);

            _db.CountryModels.Add(countryModel);
            _db.SaveChanges();
        }

        public CountryEditModel GetById(int id)
        {
            var country = _db.CountryModels.SingleOrDefault(x => x.Id == id);

            if (country == null)
            {
                return null;
            }

            return Mapper.Map<CountryEditModel>(country);
        }

        public void EditCountry(CountryEditModel countryEditModel)
        {
            var countryModel = _db.CountryModels.SingleOrDefault(x => x.Id == countryEditModel.Id);

            countryModel.Name = countryEditModel.Name;

            _db.Entry(countryModel).State = EntityState.Modified;

            _db.SaveChanges();
        }

        public void DeleteCountry(int id)
        {
            CountryModel countryModel = _db.CountryModels.Find(id);
            _db.CountryModels.Remove(countryModel);
            _db.SaveChanges();
        }
    }
}