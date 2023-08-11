using AutoMapper;
using PFM.Api.Contracts.Income;
using PFM.Website.Configurations;
using PFM.Website.ExternalServices;
using PFM.Website.Models;

namespace PFM.Website.Services
{
    public class IncomeService: CoreService
    {
        private readonly IIncomeApi _api;

        public IncomeService(Serilog.ILogger logger, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            ApplicationSettings settings, IIncomeApi api)
            : base(logger, mapper, httpContextAccessor, settings)
        {
            _api = api;
        }

        public async Task<List<IncomeListModel>> GetAll(int accountId)
        {
            var apiResponse = await _api.GetList(accountId);
            var response = ReadApiResponse<List<IncomeList>>(apiResponse) ?? new List<IncomeList>();
            return response.Select(_mapper.Map<IncomeListModel>).ToList();
        }

        public async Task<bool> Create(int accountId, IncomeEditModel model)
        {
            var request = _mapper.Map<IncomeDetails>(model);
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
    }
}

