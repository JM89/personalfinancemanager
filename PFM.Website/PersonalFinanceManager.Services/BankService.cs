using System;
using System.Collections.Generic;
using System.Linq;
using PersonalFinanceManager.Models.Bank;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Services
{
    public class BankService : IBankService
    {
        public IList<BankListModel> GetBanks()
        {
            IList<BankListModel> result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetList<PFM.DTOs.Currency.CurrencyList>($"/Bank/GetList");
                result = response.Select(AutoMapper.Mapper.Map<BankListModel>).ToList();
            }
            return result;
        }

        public void CreateBank(BankEditModel model)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PFM.DTOs.Bank.BankDetails>(model);
                httpClient.Post($"/Bank/Create", dto);
            }
        }

        public BankEditModel GetById(int id)
        {
            BankEditModel result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetSingle<PFM.DTOs.Bank.BankDetails>($"/Bank/Get/{id}");
                result = AutoMapper.Mapper.Map<BankEditModel>(response);
            }
            return result;
        }

        public void EditBank(BankEditModel model)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PFM.DTOs.Bank.BankDetails>(model);
                httpClient.Put($"/Bank/Edit/{model.Id}", dto);
            }
        }

        public void DeleteBank(int id)
        {
            using (var httpClient = new HttpClientExtended())
            {
                httpClient.Delete($"/Bank/Delete/{id}");
            }
        }
    }
}