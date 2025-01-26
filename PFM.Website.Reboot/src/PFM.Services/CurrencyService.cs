using AutoMapper;
using Microsoft.AspNetCore.Http;
using PFM.Bank.Api.Contracts.Currency;
using PFM.Models;
using PFM.Services.ExternalServices;

namespace PFM.Services
{
    public class CurrencyService(
        Serilog.ILogger logger,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        ICurrencyApi api)
        : CoreService(logger, mapper, httpContextAccessor)
    {
        public async Task<List<CurrencyModel>> GetAll()
        {
            var apiResponse = await api.GetList();
            var response = ReadApiResponse<List<CurrencyList>>(apiResponse) ?? new List<CurrencyList>();
            return response.Select(Mapper.Map<CurrencyModel>).ToList();
        }

        public async Task<CurrencyModel> GetById(int id)
        {
            var apiResponse = await api.Get(id);
            var item = ReadApiResponse<CurrencyDetails>(apiResponse);
            return Mapper.Map<CurrencyModel>(item);
        }
    }
}

