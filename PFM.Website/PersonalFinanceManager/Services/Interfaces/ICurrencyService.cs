using System.Collections.Generic;
using PersonalFinanceManager.Models.Currency;
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