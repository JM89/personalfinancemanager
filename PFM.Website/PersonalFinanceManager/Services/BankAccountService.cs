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
        private readonly IHttpClientExtended _httpClientExtended;

        public BankAccountService(Serilog.ILogger logger, IHttpClientExtended httpClientExtended)
        {
            _logger = logger;
            _httpClientExtended = httpClientExtended;
        }

        public void CreateBankAccount(AccountEditModel model, string userId)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Account.AccountDetails>(model);
            _httpClientExtended.Post($"/BankAccount/Create/{userId}", dto);
        }

        public IList<AccountListModel> GetAccountsByUser(string userId)
        {
            var response = _httpClientExtended.GetList<PFM.Api.Contracts.Account.AccountList>($"/BankAccount/GetList/{userId}");
            return response.Select(AutoMapper.Mapper.Map<AccountListModel>).ToList();
        }
        
        public AccountEditModel GetById(int id)
        {
            var response = _httpClientExtended.GetSingle<PFM.Api.Contracts.Account.AccountDetails>($"/BankAccount/Get/{id}");
            return AutoMapper.Mapper.Map<AccountEditModel>(response);
        }

        public void EditBankAccount(AccountEditModel model, string userId)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.Account.AccountDetails>(model);
            _httpClientExtended.Put($"/BankAccount/Edit/{model.Id}/{userId}", dto);
        }

        public void DeleteBankAccount(int id)
        {
            _httpClientExtended.Delete($"/BankAccount/Delete/{id}");
        }
        
        public void SetAsFavorite(int id)
        {
            _httpClientExtended.Post($"/BankAccount/SetAsFavorite/{id}");
        }
    }
}