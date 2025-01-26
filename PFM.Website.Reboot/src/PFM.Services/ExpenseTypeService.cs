using AutoMapper;
using Microsoft.AspNetCore.Http;
using PFM.Api.Contracts.ExpenseType;
using PFM.Models;
using PFM.Services.ExternalServices;

namespace PFM.Services
{
    public class ExpenseTypeService(
        Serilog.ILogger logger,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        IExpenseTypeApi api)
        : CoreService(logger, mapper, httpContextAccessor)
    {
        public async Task<List<ExpenseTypeModel>> GetAll()
        {
            var apiResponse = await api.Get(GetCurrentUserId());
            var response = ReadApiResponse<List<ExpenseTypeList>>(apiResponse) ?? new List<ExpenseTypeList>();
            return response.Select(Mapper.Map<ExpenseTypeModel>).ToList();
        }

        public async Task<ExpenseTypeModel> GetById(int id)
        {
            var apiResponse = await api.Get(id);
            var item = ReadApiResponse<ExpenseTypeDetails>(apiResponse);
            return Mapper.Map<ExpenseTypeModel>(item);
        }

        public async Task<bool> Create(ExpenseTypeModel model)
        {
            var request = Mapper.Map<ExpenseTypeDetails>(model);
            var apiResponse = await api.Create(GetCurrentUserId(), request);
            var result = ReadApiResponse<bool>(apiResponse);
            return result;
        }

        public async Task<bool> Edit(int id, ExpenseTypeModel model)
        {
            var request = Mapper.Map<ExpenseTypeDetails>(model);
            var apiResponse = await api.Edit(id, GetCurrentUserId(), request);
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

