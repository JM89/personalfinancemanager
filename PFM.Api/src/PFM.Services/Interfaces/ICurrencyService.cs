﻿using System.Collections.Generic;
using PFM.Api.Contracts.Currency;

namespace PFM.Services.Interfaces
{
    public interface ICurrencyService : IBaseService
    {
        IList<CurrencyList> GetCurrencies();

        CurrencyDetails GetById(int id);

        void CreateCurrency(CurrencyDetails currencyDetails);

        void EditCurrency(CurrencyDetails currencyDetails);

        void DeleteCurrency(int id);
    }
}