using PFM.Bank.Api.Contracts.Currency;
using System.Threading.Tasks;

namespace PFM.Services.Caches.Interfaces
{
    public interface ICurrencyCache
    {
        Task<CurrencyDetails> GetById(int id);
    }
}
