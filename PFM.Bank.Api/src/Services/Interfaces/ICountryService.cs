using PFM.Bank.Api.Contracts.Country;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface ICountryService : IBaseService
    {
        IList<CountryList> GetCountries();

        CountryDetails GetById(int id);
    }
}