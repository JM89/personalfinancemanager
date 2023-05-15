using PersonalFinanceManager.Models.Account;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<bool> CreateBankAccount(AccountEditModel model, string userId)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Bank.Api.Contracts.Account.AccountDetails>(model);
            return await _httpClientExtended.Post($"/BankAccount/Create/{userId}", dto);
        }

        public async Task<IList<AccountListModel>> GetAccountsByUser(string userId)
        {
            var response = await _httpClientExtended.GetList<PFM.Bank.Api.Contracts.Account.AccountList>($"/BankAccount/GetList/{userId}");
            return response.Select(AutoMapper.Mapper.Map<AccountListModel>).ToList();
        }
        
        public async Task<AccountEditModel> GetById(int id)
        {
            var response = await _httpClientExtended.GetSingle<PFM.Bank.Api.Contracts.Account.AccountDetails>($"/BankAccount/Get/{id}");
            return AutoMapper.Mapper.Map<AccountEditModel>(response);
        }

        public async Task<bool> EditBankAccount(AccountEditModel model, string userId)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Bank.Api.Contracts.Account.AccountDetails>(model);
            return await _httpClientExtended.Put($"/BankAccount/Edit/{model.Id}/{userId}", dto);
        }

        public async Task<bool> DeleteBankAccount(int id)
        {
            return await _httpClientExtended.Delete($"/BankAccount/Delete/{id}");
        }
        
        public async Task<bool> SetAsFavorite(int id)
        {
            return await _httpClientExtended.Post($"/BankAccount/SetAsFavorite/{id}");
        }
    }
}