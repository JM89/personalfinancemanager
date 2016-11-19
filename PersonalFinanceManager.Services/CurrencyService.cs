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
        ApplicationDbContext db;

        public CurrencyService()
        {
            db = new ApplicationDbContext();
        }

        public IList<CurrencyListModel> GetCurrencies()
        {
            var currencies = db.CurrencyModels.ToList();

            var accounts = db.AccountModels;

            var currenciesModel = currencies.Select(x => Mapper.Map<CurrencyListModel>(x)).ToList();

            currenciesModel.ForEach(currency =>
            {
                currency.CanBeDeleted = !accounts.Any(x => x.CurrencyId == currency.Id);
            });

            return currenciesModel;
        }

        public CurrencyEditModel GetById(int id)
        {
            var currency = db.CurrencyModels.SingleOrDefault(x => x.Id == id);

            if (currency == null)
            {
                return null;
            }

            return Mapper.Map<CurrencyEditModel>(currency);
        }

        public void CreateCurrency(CurrencyEditModel currencyEditModel)
        {
            var currencyModel = Mapper.Map<CurrencyModel>(currencyEditModel);

            db.CurrencyModels.Add(currencyModel);
            db.SaveChanges();
        }

        public void EditCurrency(CurrencyEditModel currencyEditModel)
        {
            var currencyModel = db.CurrencyModels.SingleOrDefault(x => x.Id == currencyEditModel.Id);
            currencyModel.Name = currencyEditModel.Name;
            currencyModel.Symbol = currencyEditModel.Symbol;

            db.Entry(currencyModel).State = EntityState.Modified;

            db.SaveChanges();
        }

        public void DeleteCurrency(int id)
        {
            CurrencyModel currencyModel = db.CurrencyModels.Find(id);
            db.CurrencyModels.Remove(currencyModel);
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}