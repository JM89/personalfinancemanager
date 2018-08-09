using System;
using System.Collections.Generic;
using System.Linq;
using PersonalFinanceManager.Models.Account;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Services
{
    public class BankAccountService: IBankAccountService
    {
        public void CreateBankAccount(AccountEditModel model, string userId)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PFM.DTOs.Currency.CurrencyDetails>(model);
                httpClient.Post($"/BankAccount/Create/{userId}", dto);
            }
        }

        public IList<AccountListModel> GetAccountsByUser(string userId)
        {
            IList<AccountListModel> result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetList<PFM.DTOs.Account.AccountList>($"/BankAccount/GetList/{userId}");
                result = response.Select(AutoMapper.Mapper.Map<AccountListModel>).ToList();
            }
            return result;
        }
        
        public AccountEditModel GetById(int id)
        {
            AccountEditModel result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetSingle<PFM.DTOs.Account.AccountDetails>($"/BankAccount/Get/{id}");
                result = AutoMapper.Mapper.Map<AccountEditModel>(response);
            }
            return result;
        }

        public void EditBankAccount(AccountEditModel model, string userId)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PFM.DTOs.Account.AccountDetails>(model);
                httpClient.Put($"/BankAccount/Edit/{model.Id}/{userId}", dto);
            }
        }

        public void DeleteBankAccount(int id)
        {
            using (var httpClient = new HttpClientExtended())
            {
                httpClient.Delete($"/BankAccount/Delete/{id}");
            }
        }
        
        public void SetAsFavorite(int id)
        {
            // API NOT IMPLEMENTED
            throw new NotImplementedException();
        }
    }
}