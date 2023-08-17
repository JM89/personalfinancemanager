using AutoMapper;
using PFM.Bank.Api.Contracts.Country;
using PFM.Website.Configurations;
using PFM.Website.ExternalServices;
using PFM.Website.Models;

namespace PFM.Website.Services
{
    public class CountryService: CoreService
    {
        private readonly ICountryApi _api;

        public CountryService(Serilog.ILogger logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, ApplicationSettings settings, ICountryApi api)
            : base(logger, mapper, httpContextAccessor, settings)
        {
            _api = api;
        }

        public async Task<List<CountryModel>> GetAll()
        {
            var apiResponse = await _api.Get();
            var response = ReadApiResponse<List<CountryList>>(apiResponse) ?? new List<CountryList>();
            return response.Select(_mapper.Map<CountryModel>).ToList();
        }

        public async Task<CountryModel> GetById(int id)
        {
            var apiResponse = await _api.Get(id);
            var item = ReadApiResponse<CountryDetails>(apiResponse);
            return _mapper.Map<CountryModel>(item);
        }
    }
}

