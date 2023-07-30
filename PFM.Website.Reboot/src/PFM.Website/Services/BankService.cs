using AutoMapper;
using PFM.Api.Contracts.ExpenseType;
using PFM.Bank.Api.Contracts.Bank;
using PFM.Website.ExternalServices;
using PFM.Website.Models;

namespace PFM.Website.Services
{
    public class BankService: CoreService
    {
        private readonly IBankApi _api;

        public BankService(Serilog.ILogger logger, IMapper mapper, IBankApi api)
            : base(logger, mapper)
        {
            _api = api;
        }

        public async Task<List<BankModel>> GetAll()
        {
            var apiResponse = await _api.Get();
            var response = ReadApiResponse<List<BankList>>(apiResponse) ?? new List<BankList>();
            return response.Select(_mapper.Map<BankModel>).ToList();
        }

        public async Task<BankModel> GetById(int id)
        {
            var apiResponse = await _api.Get(id);
            var item = ReadApiResponse<BankDetails>(apiResponse);
            return _mapper.Map<BankModel>(item);
        }

        public async Task<bool> Create(BankModel model)
        {
            var request = _mapper.Map<BankDetails>(model);
            var apiResponse = await _api.Create(request);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }

        public async Task<bool> Edit(int id, BankModel model)
        {
            var request = _mapper.Map<BankDetails>(model);
            var apiResponse = await _api.Edit(id, request);
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

