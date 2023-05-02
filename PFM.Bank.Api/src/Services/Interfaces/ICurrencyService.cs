using System.Collections.Generic;
using PFM.Api.Contracts.Currency;

namespace PFM.Services.Interfaces
{
    public interface ICurrencyService : IBaseService
    {
        IList<CurrencyList> GetCurrencies();

        CurrencyDetails GetById(int id);
    }
}