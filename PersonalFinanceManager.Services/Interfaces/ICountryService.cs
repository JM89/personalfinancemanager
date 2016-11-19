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
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface ICountryService : IBaseService
    {
        IList<CountryListModel> GetCountries();

        void CreateCountry(CountryEditModel countryEditModel);

        CountryEditModel GetById(int id);

        void EditCountry(CountryEditModel countryEditModel);

        void DeleteCountry(int id);
    }
}