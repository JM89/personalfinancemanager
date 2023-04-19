using PersonalFinanceManager.Models.Currency;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly Serilog.ILogger _logger;
        private readonly IHttpClientExtended _httpClientExtended;

        public CurrencyService(Serilog.ILogger logger, IHttpClientExtended httpClientExtended)
        {
            _logger = logger;
            _httpClientExtended = httpClientExtended;
        }

        public async Task<IList<CurrencyListModel>> GetCurrencies()
        {
            var response = await _httpClientExtended.GetList<PFM.Api.Contracts.Currency.CurrencyList>($"/Currency/GetList");
            return response.Select(AutoMapper.Mapper.Map<CurrencyListModel>).ToList();
        }

        public async Task<CurrencyEditModel> GetById(int id)
        {
            var response = await _httpClientExtended.GetSingle<PFM.Api.Contracts.Currency.CurrencyDetails>($"/Currency/Get/{id}");
            return AutoMapper.Mapper.Map<CurrencyEditModel>(response);
        }

        public async Task<bool> CreateCurrency(CurrencyEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Currency.CurrencyDetails>(model);
            return await _httpClientExtended.Post($"/Currency/Create", dto);
        }

        public async Task<bool> EditCurrency(CurrencyEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Currency.CurrencyDetails>(model);
            return await _httpClientExtended.Put($"/Currency/Edit/{model.Id}", dto);
        }

        public async Task<bool> DeleteCurrency(int id)
        {
            return await _httpClientExtended.Delete($"/Currency/Delete/{id}");
        }
    }
}