using PersonalFinanceManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Models.Currency;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using PersonalFinanceManager.DataAccess;
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Services
{
    public class CurrencyService : ICurrencyService
    {
        private ApplicationDbContext _db;

        public CurrencyService(ApplicationDbContext db)
        {
            this._db = db;
        }

        public IList<CurrencyListModel> GetCurrencies()
        {
            var currencies = _db.CurrencyModels.ToList();

            var accounts = _db.AccountModels;

            var currenciesModel = currencies.Select(x => Mapper.Map<CurrencyListModel>(x)).ToList();

            currenciesModel.ForEach(currency =>
            {
                currency.CanBeDeleted = !accounts.Any(x => x.CurrencyId == currency.Id);
            });

            return currenciesModel;
        }

        public CurrencyEditModel GetById(int id)
        {
            var currency = _db.CurrencyModels.SingleOrDefault(x => x.Id == id);

            if (currency == null)
            {
                return null;
            }

            return Mapper.Map<CurrencyEditModel>(currency);
        }

        public void CreateCurrency(CurrencyEditModel currencyEditModel)
        {
            var currencyModel = Mapper.Map<CurrencyModel>(currencyEditModel);

            _db.CurrencyModels.Add(currencyModel);
            _db.SaveChanges();
        }

        public void EditCurrency(CurrencyEditModel currencyEditModel)
        {
            var currencyModel = _db.CurrencyModels.SingleOrDefault(x => x.Id == currencyEditModel.Id);
            currencyModel.Name = currencyEditModel.Name;
            currencyModel.Symbol = currencyEditModel.Symbol;

            _db.Entry(currencyModel).State = EntityState.Modified;

            _db.SaveChanges();
        }

        public void DeleteCurrency(int id)
        {
            CurrencyModel currencyModel = _db.CurrencyModels.Find(id);
            _db.CurrencyModels.Remove(currencyModel);
            _db.SaveChanges();
        }
    }
}