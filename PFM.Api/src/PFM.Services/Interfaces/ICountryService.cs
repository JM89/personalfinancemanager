using PFM.Api.Contracts.Country;
using System.Collections.Generic;

namespace PFM.Services.Interfaces
{
    public interface ICountryService : IBaseService
    {
        IList<CountryList> GetCountries();

        CountryDetails GetById(int id);
    }
}