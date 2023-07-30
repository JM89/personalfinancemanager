using Api.Contracts.Shared;
using Newtonsoft.Json;
using PFM.Api.Contracts.ExpenseType;
using PFM.Website.ExternalServices;

namespace PFM.Website.Services
{
    public class ExpenseTypeService
    {
        private readonly IExpenseTypeApi _expenseTypeApi;
        private readonly Serilog.ILogger _logger;

        public ExpenseTypeService(Serilog.ILogger logger, IExpenseTypeApi expenseTypeApi)
        {
            _logger = logger;
            _expenseTypeApi = expenseTypeApi;
        }

        public async Task<ExpenseTypeList[]> GetAll()
        {
            var apiResponse = await _expenseTypeApi.Get();

            var results = ReadApiResponse<List<ExpenseTypeList>>(apiResponse);

            return await Task.FromResult(results.ToArray());
        }

        public async Task<ExpenseTypeDetails?> GetById(int id)
        {
            var apiResponse = await _expenseTypeApi.Get(id);

            var result = ReadApiResponse<ExpenseTypeDetails>(apiResponse);

            return await Task.FromResult(result);
        }

        public async Task<bool> Create(ExpenseTypeDetails expenseType)
        {
            var apiResponse = await _expenseTypeApi.Create(expenseType);

            var result = ReadApiResponse<bool>(apiResponse);

            return await Task.FromResult(result);
        }

        public async Task<bool> Edit(int id, ExpenseTypeDetails expenseType)
        {
            var apiResponse = await _expenseTypeApi.Edit(id, expenseType);

            var result = ReadApiResponse<bool>(apiResponse);

            return await Task.FromResult(result);
        }

        public async Task<bool> Delete(int id)
        {
            var apiResponse = await _expenseTypeApi.Delete(id);

            var result = ReadApiResponse<bool>(apiResponse);

            return await Task.FromResult(result);

        }

        private TResult? ReadApiResponse<TResult>(ApiResponse apiResponse)
        {
            if (apiResponse.Data == null)
            {
                _logger.Error("No data returned");
                return default(TResult);
            }

            _logger.Information("Read API Response of type {Type}", apiResponse.Data.GetType());

            if (typeof(TResult) == typeof(bool) && bool.TryParse(apiResponse.Data.ToString(), out bool bResult))
            {
                return (TResult)Convert.ChangeType(bResult, typeof(TResult));
            }

            return JsonConvert.DeserializeObject<TResult>(apiResponse.Data.ToString() ?? "");
        }
    }
}

