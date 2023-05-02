using PFM.Bank.Api.Contracts.Currency;
using System.Collections.Generic;

namespace PFM.Services.Interfaces
{
    public interface ICurrencyService : IBaseService
    {
        IList<CurrencyList> GetCurrencies();

        CurrencyDetails GetById(int id);
    }
}