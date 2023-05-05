using PFM.Bank.Api.Contracts.Country;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICountryService : IBaseService
    {
        Task<List<CountryList>> GetCountries();

        Task<CountryDetails> GetById(int id);
    }
}