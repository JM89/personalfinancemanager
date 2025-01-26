using AutoMapper;
using Microsoft.AspNetCore.Http;
using PFM.Api.Contracts.Income;
using PFM.Models;
using PFM.Services.ExternalServices;

namespace PFM.Services
{
    public class IncomeService(
        Serilog.ILogger logger,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        IIncomeApi api)
        : CoreService(logger, mapper, httpContextAccessor)
    {
        public async Task<List<IncomeListModel>> GetAll(int accountId)
        {
            var apiResponse = await api.GetList(accountId);
            var response = ReadApiResponse<List<IncomeList>>(apiResponse) ?? new List<IncomeList>();
            return response.Select(Mapper.Map<IncomeListModel>).ToList();
        }

        public async Task<bool> Create(int accountId, IncomeEditModel model)
        {
            var request = Mapper.Map<IncomeDetails>(model);
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
    }
}

