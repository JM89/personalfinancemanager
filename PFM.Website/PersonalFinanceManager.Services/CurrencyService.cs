using System.Collections.Generic;
using PersonalFinanceManager.Models.Currency;
using PersonalFinanceManager.Services.Interfaces;
using System;
using PersonalFinanceManager.Services.HttpClientWrapper;
using System.Linq;

namespace PersonalFinanceManager.Services
{
    public class CurrencyService : ICurrencyService
    {
        public IList<CurrencyListModel> GetCurrencies()
        {
            IList<CurrencyListModel> result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetList<PFM.DTOs.Currency.CurrencyList>($"/Currency/GetList");
                result = response.Select(AutoMapper.Mapper.Map<CurrencyListModel>).ToList();
            }
            return result;
        }

        public CurrencyEditModel GetById(int id)
        {
            CurrencyEditModel result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetSingle<PFM.DTOs.Currency.CurrencyDetails>($"/Currency/Get/{id}");
                result = AutoMapper.Mapper.Map<CurrencyEditModel>(response);
            }
            return result;
        }

        public void CreateCurrency(CurrencyEditModel model)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PFM.DTOs.Currency.CurrencyDetails>(model);
                httpClient.Post($"/Currency/Create", dto);
            }
        }

        public void EditCurrency(CurrencyEditModel model)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PFM.DTOs.Currency.CurrencyDetails>(model);
                httpClient.Put($"/Currency/Edit/{model.Id}", dto);
            }
        }

        public void DeleteCurrency(int id)
        {
            using (var httpClient = new HttpClientExtended())
            {
                httpClient.Delete($"/Currency/Delete/{id}");
            }
        }
    }
}