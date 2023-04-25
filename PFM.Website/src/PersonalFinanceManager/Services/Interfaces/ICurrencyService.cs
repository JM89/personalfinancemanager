using System.Collections.Generic;
using System.Threading.Tasks;
using PersonalFinanceManager.Models.Currency;
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface ICurrencyService : IBaseService
    {
        Task<IList<CurrencyListModel>> GetCurrencies();

        Task<CurrencyEditModel> GetById(int id);

        Task<bool> CreateCurrency(CurrencyEditModel currencyEditModel);

        Task<bool> EditCurrency(CurrencyEditModel currencyEditModel);

        Task<bool> DeleteCurrency(int id);
    }
}