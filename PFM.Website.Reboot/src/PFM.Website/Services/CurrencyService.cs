using AutoMapper;
using PFM.Bank.Api.Contracts.Currency;
using PFM.Website.Configurations;
using PFM.Website.ExternalServices;
using PFM.Website.Models;

namespace PFM.Website.Services
{
    public class CurrencyService: CoreService
    {
        private readonly ICurrencyApi _api;

        public CurrencyService(Serilog.ILogger logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, ApplicationSettings settings, ICurrencyApi api)
            : base(logger, mapper, httpContextAccessor, settings)
        {
            _api = api;
        }

        public async Task<List<CurrencyModel>> GetAll()
        {
            var apiResponse = await _api.GetList();
            var response = ReadApiResponse<List<CurrencyList>>(apiResponse) ?? new List<CurrencyList>();
            return response.Select(_mapper.Map<CurrencyModel>).ToList();
        }

        public async Task<CurrencyModel> GetById(int id)
        {
            var apiResponse = await _api.Get(id);
            var item = ReadApiResponse<CurrencyDetails>(apiResponse);
            return _mapper.Map<CurrencyModel>(item);
        }
    }
}

