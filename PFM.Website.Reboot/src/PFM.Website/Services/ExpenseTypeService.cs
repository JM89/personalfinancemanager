using System.Text.Json;
using Api.Contracts.Shared;
using PFM.Api.Contracts.ExpenseType;
using PFM.Website.ExternalServices;
using Refit;

namespace PFM.Website.Services
{
	public class ExpenseTypeService
	{
        private readonly IExpenseTypeApi _expenseTypeApi;

        public ExpenseTypeService(IExpenseTypeApi expenseTypeApi)
        {
            _expenseTypeApi = expenseTypeApi;
        }

        public async Task<ExpenseTypeList[]> GetAll()
        {
            var apiResponse = await _expenseTypeApi.Get();

            if (apiResponse.Data == null)
            {
                throw new NotImplementedException("todo");
            }

            var results = ReadApiResponse<IList<ExpenseTypeList>>(apiResponse);

            return await Task.FromResult(results.ToArray());
        }

        public async Task<ExpenseTypeDetails?> GetById(int id)
        {
            var apiResponse = await _expenseTypeApi.Get(id);

            if (apiResponse.Data == null)
            {
                throw new NotImplementedException("todo");
            }

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
                throw new NotImplementedException();
            }

            if (typeof(TResult) == typeof(bool) && bool.TryParse(apiResponse.Data.ToString(), out bool bResult))
            {
                return (TResult)Convert.ChangeType(bResult, typeof(TResult));
            }

            return JsonSerializer.Deserialize<TResult>(JsonSerializer.Serialize(apiResponse.Data));
        }
    }
}

