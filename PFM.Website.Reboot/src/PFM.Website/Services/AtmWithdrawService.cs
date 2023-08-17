using AutoMapper;
using PFM.Api.Contracts.AtmWithdraw;
using PFM.Website.Configurations;
using PFM.Website.ExternalServices;
using PFM.Website.Models;

namespace PFM.Website.Services
{
    public class AtmWithdrawService: CoreService
    {
        private readonly IAtmWithdrawApi _api;

        public AtmWithdrawService(Serilog.ILogger logger, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            ApplicationSettings settings, IAtmWithdrawApi api)
            : base(logger, mapper, httpContextAccessor, settings)
        {
            _api = api;
        }

        public async Task<List<AtmWithdrawListModel>> GetAll(int accountId)
        {
            var apiResponse = await _api.GetList(accountId);
            var response = ReadApiResponse<List<AtmWithdrawList>>(apiResponse) ?? new List<AtmWithdrawList>();
            return response.Select(_mapper.Map<AtmWithdrawListModel>).ToList();
        }

        public async Task<bool> Create(int accountId, AtmWithdrawEditModel model)
        {
            var request = _mapper.Map<AtmWithdrawDetails>(model);
            request.AccountId = accountId;
            var apiResponse = await _api.Create(request);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }

        public async Task<bool> Delete(int id)
        {
            var apiResponse = await _api.Delete(id);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }

        public async Task<bool> ChangeDebitStatus(int id, bool debitStatus)
        {
            var apiResponse = await _api.ChangeDebitStatus(id, debitStatus);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }

        public async Task<bool> CloseAtmWithdraw(int id)
        {
            var apiResponse = await _api.CloseAtmWithdraw(id);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }
    }
}

