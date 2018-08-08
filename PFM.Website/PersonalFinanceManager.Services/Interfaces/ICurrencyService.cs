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
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface ICurrencyService : IBaseService
    {
        IList<CurrencyListModel> GetCurrencies();

        CurrencyEditModel GetById(int id);

        void CreateCurrency(CurrencyEditModel currencyEditModel);

        void EditCurrency(CurrencyEditModel currencyEditModel);

        void DeleteCurrency(int id);
    }
}