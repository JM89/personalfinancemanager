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
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;

namespace PersonalFinanceManager.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IBankAccountRepository _bankAccountRepository;

        public CurrencyService(ICurrencyRepository currencyRepository, IBankAccountRepository bankAccountRepository)
        {
            this._currencyRepository = currencyRepository;
            this._bankAccountRepository = bankAccountRepository;
        }

        public IList<CurrencyListModel> GetCurrencies()
        {
            var currencies = _currencyRepository.GetList().ToList();

            var accounts = _bankAccountRepository.GetList();

            var currenciesModel = currencies.Select(x => Mapper.Map<CurrencyListModel>(x)).ToList();

            currenciesModel.ForEach(currency =>
            {
                currency.CanBeDeleted = !accounts.Any(x => x.CurrencyId == currency.Id);
            });

            return currenciesModel;
        }

        public CurrencyEditModel GetById(int id)
        {
            var currency = _currencyRepository.GetById(id);

            if (currency == null)
            {
                return null;
            }

            return Mapper.Map<CurrencyEditModel>(currency);
        }

        public void CreateCurrency(CurrencyEditModel currencyEditModel)
        {
            var currencyModel = Mapper.Map<CurrencyModel>(currencyEditModel);
            _currencyRepository.Create(currencyModel);
        }

        public void EditCurrency(CurrencyEditModel currencyEditModel)
        {
            var currencyModel = _currencyRepository.GetById(currencyEditModel.Id); 
            currencyModel.Name = currencyEditModel.Name;
            currencyModel.Symbol = currencyEditModel.Symbol;
            _currencyRepository.Update(currencyModel);
        }

        public void DeleteCurrency(int id)
        {
            var currencyModel = _currencyRepository.GetById(id);
            _currencyRepository.Delete(currencyModel);
        }
    }
}