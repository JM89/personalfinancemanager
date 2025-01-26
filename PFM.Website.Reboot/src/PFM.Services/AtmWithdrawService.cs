using AutoMapper;
using Microsoft.AspNetCore.Http;
using PFM.Api.Contracts.AtmWithdraw;
using PFM.Models;
using PFM.Services.ExternalServices;

namespace PFM.Services
{
    public class AtmWithdrawService(
        Serilog.ILogger logger,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        IAtmWithdrawApi api)
        : CoreService(logger, mapper, httpContextAccessor)
    {
        public async Task<List<AtmWithdrawListModel>> GetAll(int accountId)
        {
            var apiResponse = await api.GetList(accountId);
            var response = ReadApiResponse<List<AtmWithdrawList>>(apiResponse) ?? new List<AtmWithdrawList>();
            return response.Select(Mapper.Map<AtmWithdrawListModel>).ToList();
        }

        public async Task<bool> Create(int accountId, AtmWithdrawEditModel model)
        {
            var request = Mapper.Map<AtmWithdrawDetails>(model);
            request.AccountId = accountId;
            var apiResponse = await api.Create(request);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }

        public async Task<bool> Delete(int id)
        {
            var apiResponse = await api.Delete(id);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }

        public async Task<bool> ChangeDebitStatus(int id, bool debitStatus)
        {
            var apiResponse = await api.ChangeDebitStatus(id, debitStatus);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }

        public async Task<bool> CloseAtmWithdraw(int id)
        {
            var apiResponse = await api.CloseAtmWithdraw(id);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }
    }
}

