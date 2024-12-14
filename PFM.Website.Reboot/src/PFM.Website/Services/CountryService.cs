using AutoMapper;
using PFM.Bank.Api.Contracts.Country;
using PFM.Website.Configurations;
using PFM.Website.ExternalServices;
using PFM.Website.Models;

namespace PFM.Website.Services
{
    public class CountryService(
        Serilog.ILogger logger,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        ApplicationSettings settings,
        ICountryApi api)
        : CoreService(logger, mapper, httpContextAccessor, settings)
    {
        public async Task<List<CountryModel>> GetAll()
        {
            var apiResponse = await api.Get();
            var response = ReadApiResponse<List<CountryList>>(apiResponse) ?? new List<CountryList>();
            return response.Select(Mapper.Map<CountryModel>).ToList();
        }

        public async Task<CountryModel> GetById(int id)
        {
            var apiResponse = await api.Get(id);
            var item = ReadApiResponse<CountryDetails>(apiResponse);
            return Mapper.Map<CountryModel>(item);
        }
    }
}

