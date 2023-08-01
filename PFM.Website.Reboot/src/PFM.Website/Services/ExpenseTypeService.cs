using AutoMapper;
using PFM.Api.Contracts.ExpenseType;
using PFM.Website.Configurations;
using PFM.Website.ExternalServices;
using PFM.Website.Models;

namespace PFM.Website.Services
{
    public class ExpenseTypeService: CoreService
    {
        private readonly IExpenseTypeApi _api;

        public ExpenseTypeService(Serilog.ILogger logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, ApplicationSettings settings, IExpenseTypeApi api)
            : base(logger, mapper, httpContextAccessor, settings)
        {
            _api = api;
        }

        public async Task<List<ExpenseTypeModel>> GetAll()
        {
            var apiResponse = await _api.Get();
            var response = ReadApiResponse<List<ExpenseTypeList>>(apiResponse) ?? new List<ExpenseTypeList>();
            return response.Select(_mapper.Map<ExpenseTypeModel>).ToList();
        }

        public async Task<ExpenseTypeModel> GetById(int id)
        {
            var apiResponse = await _api.Get(id);
            var item = ReadApiResponse<ExpenseTypeDetails>(apiResponse);
            return _mapper.Map<ExpenseTypeModel>(item);
        }

        public async Task<bool> Create(ExpenseTypeModel model)
        {
            var request = _mapper.Map<ExpenseTypeDetails>(model);
            var apiResponse = await _api.Create(request);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }

        public async Task<bool> Edit(int id, ExpenseTypeModel model)
        {
            var request = _mapper.Map<ExpenseTypeDetails>(model);
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

