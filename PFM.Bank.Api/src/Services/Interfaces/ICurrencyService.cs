using PFM.Bank.Api.Contracts.Currency;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICurrencyService : IBaseService
    {
        Task<List<CurrencyList>> GetCurrencies();

        Task<CurrencyDetails> GetById(int id);
    }
}