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
        private readonly Serilog.ILogger _logger;

        public BankAccountService(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public void CreateBankAccount(AccountEditModel model, string userId)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Account.AccountDetails>(model);
                httpClient.Post($"/BankAccount/Create/{userId}", dto);
            }
        }

        public IList<AccountListModel> GetAccountsByUser(string userId)
        {
            IList<AccountListModel> result = null;
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var response = httpClient.GetList<PFM.Api.Contracts.Account.AccountList>($"/BankAccount/GetList/{userId}");
                result = response.Select(AutoMapper.Mapper.Map<AccountListModel>).ToList();
            }
            return result;
        }
        
        public AccountEditModel GetById(int id)
        {
            AccountEditModel result = null;
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var response = httpClient.GetSingle<PFM.Api.Contracts.Account.AccountDetails>($"/BankAccount/Get/{id}");
                result = AutoMapper.Mapper.Map<AccountEditModel>(response);
            }
            return result;
        }

        public void EditBankAccount(AccountEditModel model, string userId)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Account.AccountDetails>(model);
                httpClient.Put($"/BankAccount/Edit/{model.Id}/{userId}", dto);
            }
        }

        public void DeleteBankAccount(int id)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                httpClient.Delete($"/BankAccount/Delete/{id}");
            }
        }
        
        public void SetAsFavorite(int id)
        {
            using (var httpClient = new HttpClientExtended(_logger))
            {
                httpClient.Post($"/BankAccount/SetAsFavorite/{id}");
            }
        }
    }
}