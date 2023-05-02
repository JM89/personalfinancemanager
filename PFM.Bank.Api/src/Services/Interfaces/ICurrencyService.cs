using PFM.Bank.Api.Contracts.Currency;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface ICurrencyService : IBaseService
    {
        IList<CurrencyList> GetCurrencies();

        CurrencyDetails GetById(int id);
    }
}