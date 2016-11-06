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

namespace PersonalFinanceManager.Services
{
    public class CountryService
    {
        ApplicationDbContext db;

        public CountryService()
        {
            db = new ApplicationDbContext();
        }

        public IList<CountryListModel> GetCountries()
        {
            var countries = db.CountryModels.ToList();

            return countries.Select(x => Mapper.Map<CountryListModel>(x)).ToList();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public void CreateCountry(CountryEditModel countryEditModel)
        {
            var countryModel = Mapper.Map<CountryModel>(countryEditModel);

            db.CountryModels.Add(countryModel);
            db.SaveChanges();
        }

        public CountryEditModel GetById(int id)
        {
            var country = db.CountryModels.SingleOrDefault(x => x.Id == id);

            if (country == null)
            {
                return null;
            }

            return Mapper.Map<CountryEditModel>(country);
        }

        public void EditCountry(CountryEditModel countryEditModel)
        {
            var countryModel = db.CountryModels.SingleOrDefault(x => x.Id == countryEditModel.Id);

            countryModel.Name = countryEditModel.Name;

            db.Entry(countryModel).State = EntityState.Modified;

            db.SaveChanges();
        }

        public void DeleteCountry(int id)
        {
            CountryModel countryModel = db.CountryModels.Find(id);
            db.CountryModels.Remove(countryModel);
            db.SaveChanges();
        }
    }
}