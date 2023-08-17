using AutoMapper;
using PFM.Api.Contracts.Saving;
using PFM.Website.Configurations;
using PFM.Website.ExternalServices;
using PFM.Website.Models;

namespace PFM.Website.Services
{
    public class SavingService: CoreService
    {
        private readonly ISavingApi _api;

        public SavingService(Serilog.ILogger logger, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            ApplicationSettings settings, ISavingApi api)
            : base(logger, mapper, httpContextAccessor, settings)
        {
            _api = api;
        }

        public async Task<List<SavingListModel>> GetAll(int accountId)
        {
            var apiResponse = await _api.GetList(accountId);
            var response = ReadApiResponse<List<SavingList>>(apiResponse) ?? new List<SavingList>();
            return response.Select(_mapper.Map<SavingListModel>).ToList();
        }

        public async Task<bool> Create(int accountId, SavingEditModel model)
        {
            var request = _mapper.Map<SavingDetails>(model);
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

