using AutoMapper;
using Microsoft.AspNetCore.Http;
using PFM.Bank.Api.Contracts.Account;
using PFM.Models;
using PFM.Services.ExternalServices;

namespace PFM.Services
{
    public class BankAccountService(
        Serilog.ILogger logger,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        IBankAccountApi api)
        : CoreService(logger, mapper, httpContextAccessor)
    {
        public async Task<List<BankAccountListModel>> GetAll()
        {
            var apiResponse = await api.GetList(GetCurrentUserId());
            var response = ReadApiResponse<List<AccountList>>(apiResponse) ?? new List<AccountList>();
            var models = response.Select(Mapper.Map<BankAccountListModel>).ToList();
            return models;
        }

        public async Task<BankAccountEditModel> GetById(int id)
        {
            var apiResponse = await api.Get(id);
            var item = ReadApiResponse<AccountDetails>(apiResponse);
            return Mapper.Map<BankAccountEditModel>(item);
        }

        public async Task<bool> Create(BankAccountEditModel model)
        {
            var request = Mapper.Map<AccountDetails>(model);
            var apiResponse = await api.Create(GetCurrentUserId(), request);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }

        public async Task<bool> SetAsFavorite(int id)
        {
            var apiResponse = await api.SetAsFavorite(id);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }

        public async Task<bool> Delete(int id)
        {
            var apiResponse = await api.Delete(id);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }

        public async Task<BankAccountListModel?> GetCurrentAccount(int? selectedAccount = null)
        {
            var apiResponse = await api.GetList(GetCurrentUserId());
            var response = ReadApiResponse<List<AccountList>>(apiResponse) ?? new List<AccountList>();

            AccountList? selected = null;
            if (selectedAccount.HasValue)
            {
                selected = response.SingleOrDefault(x => x.Id == selectedAccount);
            }
            else
            {
                selected = response.Where(x => !x.IsSavingAccount).OrderByDescending(x => x.IsFavorite).FirstOrDefault();
            }

            if (selected == null)
            {
                return null;
            }

            return Mapper.Map<BankAccountListModel>(selected);
        }
    }
}

