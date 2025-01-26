using AutoMapper;
using Microsoft.AspNetCore.Http;
using PFM.Api.Contracts.Expense;
using PFM.Api.Contracts.SearchParameters;
using PFM.Models;
using PFM.Services.ExternalServices;

namespace PFM.Services
{
    public class ExpenseService(
        Serilog.ILogger logger,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        IExpenseApi api)
        : CoreService(logger, mapper, httpContextAccessor)
    {
        public async Task<List<ExpenseListModel>> GetAll(ExpenseSearchParamModel search)
        {
            var request = Mapper.Map<ExpenseGetListSearchParameters>(search);
            var apiResponse = await api.GetList(request);
            var response = ReadApiResponse<List<ExpenseList>>(apiResponse) ?? new List<ExpenseList>();
            return response.Select(Mapper.Map<ExpenseListModel>).ToList();
        }

        public async Task<PagedModel<ExpenseListModel>> GetPaged(int skip, int take, ExpenseSearchParamModel search)
        {
            var request = Mapper.Map<ExpenseGetListSearchParameters>(search);
            var apiResponse = await api.GetList(request);
            var response = ReadApiResponse<List<ExpenseList>>(apiResponse) ?? new List<ExpenseList>();

            // Temporary doing server paging here, should be done in the API.
            // START PATCH--------------------------------------
            var pagedResponse = response.Skip(skip).Take(take);
            var countResponse = response.Count;
            // END PATCH--------------------------------------

            var models = pagedResponse.Select(Mapper.Map<ExpenseListModel>).ToList();
            return new PagedModel<ExpenseListModel>(models, countResponse);
        }

        public async Task<bool> Create(int accountId, ExpenseEditModel model)
        {
            var request = Mapper.Map<ExpenseDetails>(model);
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
    }
}

