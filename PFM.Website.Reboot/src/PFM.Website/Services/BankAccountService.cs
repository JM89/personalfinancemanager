using AutoMapper;
using PFM.Bank.Api.Contracts.Account;
using PFM.Website.Configurations;
using PFM.Website.ExternalServices;
using PFM.Website.Models;

namespace PFM.Website.Services
{
    public class BankAccountService: CoreService
    {
        private readonly IBankAccountApi _api;

        public BankAccountService(Serilog.ILogger logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, ApplicationSettings settings, IBankAccountApi api)
            : base(logger, mapper, httpContextAccessor, settings)
        {
            _api = api;
        }

        public async Task<List<BankAccountListModel>> GetAll()
        {
            var apiResponse = await _api.GetList(CurrentUserId);
            var response = ReadApiResponse<List<AccountList>>(apiResponse) ?? new List<AccountList>();
            var models = response.Select(_mapper.Map<BankAccountListModel>).ToList();
            return models;
        }

        public async Task<bool> Create(BankAccountEditModel model)
        {
            var request = _mapper.Map<AccountDetails>(model);
            var apiResponse = await _api.Create(CurrentUserId, request);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }

        public async Task<bool> SetAsFavorite(int id)
        {
            var apiResponse = await _api.SetAsFavorite(id);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }

        public async Task<bool> Delete(int id)
        {
            var apiResponse = await _api.Delete(id);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }
    }
}

