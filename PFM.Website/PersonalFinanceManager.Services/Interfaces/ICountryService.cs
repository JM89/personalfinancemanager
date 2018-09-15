using System.Collections.Generic;
using PersonalFinanceManager.Models.Country;
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface ICountryService : IBaseService
    {
        IList<CountryListModel> GetCountries();

        void CreateCountry(CountryEditModel countryEditModel);

        CountryEditModel GetById(int id);

        void EditCountry(CountryEditModel countryEditModel);

        void DeleteCountry(int id);
    }
}