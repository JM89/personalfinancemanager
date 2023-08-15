using AutoMapper;
using PFM.Api.Contracts.Expense;
using PFM.Api.Contracts.SearchParameters;
using PFM.Website.Configurations;
using PFM.Website.ExternalServices;
using PFM.Website.Models;

namespace PFM.Website.Services
{
    public class ExpenseService: CoreService
    {
        private readonly IExpenseApi _api;

        public ExpenseService(Serilog.ILogger logger, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            ApplicationSettings settings, IExpenseApi api)
            : base(logger, mapper, httpContextAccessor, settings)
        {
            _api = api;
        }

        public async Task<List<ExpenseListModel>> GetAll(ExpenseSearchParamModel search)
        {
            var request = _mapper.Map<ExpenseGetListSearchParameters>(search);
            var apiResponse = await _api.GetList(request);
            var response = ReadApiResponse<List<ExpenseList>>(apiResponse) ?? new List<ExpenseList>();
            return response.Select(_mapper.Map<ExpenseListModel>).ToList();
        }

        public async Task<PagedModel<ExpenseListModel>> GetPaged(int skip, int take, ExpenseSearchParamModel search)
        {
            var request = _mapper.Map<ExpenseGetListSearchParameters>(search);
            var apiResponse = await _api.GetList(request);
            var response = ReadApiResponse<List<ExpenseList>>(apiResponse) ?? new List<ExpenseList>();

            // Temporary doing server paging here, should be done in the API.
            // START PATCH--------------------------------------
            var pagedResponse = response.Skip(skip).Take(take);
            var countResponse = response.Count;
            // END PATCH--------------------------------------

            var models = pagedResponse.Select(_mapper.Map<ExpenseListModel>).ToList();
            return new PagedModel<ExpenseListModel>(models, countResponse);
        }

        public async Task<bool> Create(int accountId, ExpenseEditModel model)
        {
            var request = _mapper.Map<ExpenseDetails>(model);
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
    }
}

