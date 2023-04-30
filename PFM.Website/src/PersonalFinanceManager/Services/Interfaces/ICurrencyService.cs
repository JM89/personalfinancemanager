using PersonalFinanceManager.Models.Currency;
using PersonalFinanceManager.Services.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface ICurrencyService : IBaseService
    {
        Task<IList<CurrencyListModel>> GetCurrencies();

        Task<CurrencyEditModel> GetById(int id);
    }
}