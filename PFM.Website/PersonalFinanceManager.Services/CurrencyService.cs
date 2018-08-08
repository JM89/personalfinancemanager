using System.Collections.Generic;
using PersonalFinanceManager.Models.Currency;
using PersonalFinanceManager.Services.Interfaces;
using System;

namespace PersonalFinanceManager.Services
{
    public class CurrencyService : ICurrencyService
    {
        public IList<CurrencyListModel> GetCurrencies()
        {
            throw new NotImplementedException();
        }

        public CurrencyEditModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void CreateCurrency(CurrencyEditModel currencyEditModel)
        {
            throw new NotImplementedException();
        }

        public void EditCurrency(CurrencyEditModel currencyEditModel)
        {
            throw new NotImplementedException();
        }

        public void DeleteCurrency(int id)
        {
            throw new NotImplementedException();
        }
    }
}