using PersonalFinanceManager.Models.Currency;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

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

        public IList<CurrencyListModel> GetCurrencies()
        {
            var response = _httpClientExtended.GetList<PFM.Api.Contracts.Currency.CurrencyList>($"/Currency/GetList");
            return response.Select(AutoMapper.Mapper.Map<CurrencyListModel>).ToList();
        }

        public CurrencyEditModel GetById(int id)
        {
            var response = _httpClientExtended.GetSingle<PFM.Api.Contracts.Currency.CurrencyDetails>($"/Currency/Get/{id}");
            return AutoMapper.Mapper.Map<CurrencyEditModel>(response);
        }

        public void CreateCurrency(CurrencyEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Currency.CurrencyDetails>(model);
            _httpClientExtended.Post($"/Currency/Create", dto);
        }

        public void EditCurrency(CurrencyEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Currency.CurrencyDetails>(model);
            _httpClientExtended.Put($"/Currency/Edit/{model.Id}", dto);
        }

        public void DeleteCurrency(int id)
        {
            _httpClientExtended.Delete($"/Currency/Delete/{id}");
        }
    }
}