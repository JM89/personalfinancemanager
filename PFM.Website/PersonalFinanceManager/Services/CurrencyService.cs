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

        public CurrencyService(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public IList<CurrencyListModel> GetCurrencies()
        {
            IList<CurrencyListModel> result = null;
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var response = httpClient.GetList<PFM.Api.Contracts.Currency.CurrencyList>($"/Currency/GetList");
                result = response.Select(AutoMapper.Mapper.Map<CurrencyListModel>).ToList();
            }
            return result;
        }

        public CurrencyEditModel GetById(int id)
        {
            CurrencyEditModel result = null;
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var response = httpClient.GetSingle<PFM.Api.Contracts.Currency.CurrencyDetails>($"/Currency/Get/{id}");
                result = AutoMapper.Mapper.Map<CurrencyEditModel>(response);
            }
            return result;
        }

        public void CreateCurrency(CurrencyEditModel model)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Currency.CurrencyDetails>(model);
                httpClient.Post($"/Currency/Create", dto);
            }
        }

        public void EditCurrency(CurrencyEditModel model)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Currency.CurrencyDetails>(model);
                httpClient.Put($"/Currency/Edit/{model.Id}", dto);
            }
        }

        public void DeleteCurrency(int id)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                httpClient.Delete($"/Currency/Delete/{id}");
            }
        }
    }
}