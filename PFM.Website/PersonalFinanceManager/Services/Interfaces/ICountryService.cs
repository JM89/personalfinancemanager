using System.Collections.Generic;
using System.Threading.Tasks;
using PersonalFinanceManager.Models.Country;
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface ICountryService : IBaseService
    {
        Task<IList<CountryListModel>> GetCountries();

        Task<bool> CreateCountry(CountryEditModel countryEditModel);

        Task<CountryEditModel> GetById(int id);

        Task<bool> EditCountry(CountryEditModel countryEditModel);

        Task<bool> DeleteCountry(int id);
    }
}