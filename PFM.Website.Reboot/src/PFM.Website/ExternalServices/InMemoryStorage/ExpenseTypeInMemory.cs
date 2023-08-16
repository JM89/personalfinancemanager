using Api.Contracts.Shared;
using Newtonsoft.Json;
using PFM.Api.Contracts.ExpenseType;

namespace PFM.Website.ExternalServices.InMemoryStorage
{
    public class ExpenseTypeInMemory : IExpenseTypeApi
    {
        internal IList<ExpenseTypeDetails> _storage = new List<ExpenseTypeDetails>()
        {
            new ExpenseTypeDetails() { Id = 1, Name = "Groceries", GraphColor = "3399FF", ShowOnDashboard = true },
            new ExpenseTypeDetails() { Id = 2, Name = "Energy", GraphColor = "33CC33", ShowOnDashboard = true },
            new ExpenseTypeDetails() { Id = 3, Name = "Transport", GraphColor = "FF0000", ShowOnDashboard = true },
            new ExpenseTypeDetails() { Id = 4, Name = "Healthcare & Wellbeing", GraphColor = "0066CC", ShowOnDashboard = true },
            new ExpenseTypeDetails() { Id = 5, Name = "Leisure", GraphColor = "CCFFCC", ShowOnDashboard = true },
            new ExpenseTypeDetails() { Id = 6, Name = "Telephone, TV, Internet", GraphColor = "E5CCFF", ShowOnDashboard = true },
            new ExpenseTypeDetails() { Id = 7, Name = "Mortgage & Rent", GraphColor = "FF6666", ShowOnDashboard = true },
            new ExpenseTypeDetails() { Id = 8, Name = "House", GraphColor = "FFCCE5", ShowOnDashboard = true },
            new ExpenseTypeDetails() { Id = 9, Name = "Savings", GraphColor = "000000", ShowOnDashboard = false }
        };

        public async Task<ApiResponse> Create(ExpenseTypeDetails obj)
        {
            obj.Id = _storage.Max(x => x.Id) + 1;
            _storage.Add(obj);
            return await Task.FromResult(new ApiResponse(true));
        }

        public async Task<ApiResponse> Delete(int id)
        {
            var item = _storage.SingleOrDefault(x => x.Id == id);

            if (item == null)
                return await Task.FromResult(new ApiResponse(false));

            _storage.Remove(item);
            return await Task.FromResult(new ApiResponse(true));
        }

        public async Task<ApiResponse> Edit(int id, ExpenseTypeDetails obj)
        {
            var existing = _storage.SingleOrDefault(x => x.Id == id);

            if (existing == null)
                return await Task.FromResult(new ApiResponse(false));

            existing.Name = obj.Name;
            existing.GraphColor = obj.GraphColor;
            existing.ShowOnDashboard = obj.ShowOnDashboard;
            return await Task.FromResult(new ApiResponse(true));
        }

        public async Task<ApiResponse> Get()
        {
            var result = JsonConvert.SerializeObject(_storage.ToList());
            return await Task.FromResult(new ApiResponse((object)result));
        }

        public async Task<ApiResponse> Get(int id)
        {
            var item = JsonConvert.SerializeObject(_storage.SingleOrDefault(x => x.Id == id));
            return await Task.FromResult(new ApiResponse((object)item));
        }
    }
}

