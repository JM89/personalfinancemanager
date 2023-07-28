using System;
using Api.Contracts.Shared;
using PFM.Api.Contracts.ExpenseType;

namespace PFM.Website.ExternalServices.InMemoryStorage
{
    public class ExpenseTypeInMemory : IExpenseTypeApi
    {
        private IList<ExpenseTypeDetails> _expenseTypes;

        public ExpenseTypeInMemory()
        {
            _expenseTypes = new List<ExpenseTypeDetails>()
            {
                new ExpenseTypeDetails()
                {
                    Id = 1,
                    Name = "Food",
                    GraphColor = "3399FF",
                    ShowOnDashboard = false
                },
                new ExpenseTypeDetails()
                {
                    Id = 2,
                    Name = "Energy",
                    GraphColor = "33CC33",
                    ShowOnDashboard = false
                },
                new ExpenseTypeDetails()
                {
                    Id = 3,
                    Name = "Transport",
                    GraphColor = "FF0000",
                    ShowOnDashboard = false
                }
            };
        }

        public async Task<ApiResponse> Create(ExpenseTypeDetails obj)
        {
            obj.Id = _expenseTypes.Max(x => x.Id) + 1;
            _expenseTypes.Add(obj);
            return await Task.FromResult(new ApiResponse(true));
        }

        public async Task<ApiResponse> Delete(int id)
        {
            var item = _expenseTypes.SingleOrDefault(x => x.Id == id);

            if (item == null)
                return await Task.FromResult(new ApiResponse(false));

            _expenseTypes.Remove(item);
            return await Task.FromResult(new ApiResponse(true));
        }

        public async Task<ApiResponse> Edit(int id, ExpenseTypeDetails obj)
        {
            var existing = await GetById(id);

            if (existing == null)
                return await Task.FromResult(new ApiResponse(false));

            existing.Name = obj.Name;
            existing.GraphColor = obj.GraphColor;
            existing.ShowOnDashboard = obj.ShowOnDashboard;
            return await Task.FromResult(new ApiResponse(true));
        }

        public async Task<ApiResponse> Get()
        {
            var all = _expenseTypes.ToArray();
            return await Task.FromResult(new ApiResponse(all));
        }

        public async Task<ApiResponse> Get(int id)
        {
            var item = _expenseTypes.SingleOrDefault(x => x.Id == id);
            return await Task.FromResult(new ApiResponse(item));
        }


        public async Task<ExpenseTypeDetails[]> GetAll()
        {
            return await Task.FromResult(_expenseTypes.ToArray());
        }

        public async Task<ExpenseTypeDetails?> GetById(int id)
        {
            return await Task.FromResult(_expenseTypes.SingleOrDefault(x => x.Id == id));
        }
    }
}

