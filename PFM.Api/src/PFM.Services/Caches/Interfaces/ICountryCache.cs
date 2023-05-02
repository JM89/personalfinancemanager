using PFM.Bank.Api.Contracts.Country;
using System.Threading.Tasks;

namespace PFM.Services.Caches.Interfaces
{
    public interface ICountryCache
    {
        Task<CountryDetails> GetById(int id);
    }
}
