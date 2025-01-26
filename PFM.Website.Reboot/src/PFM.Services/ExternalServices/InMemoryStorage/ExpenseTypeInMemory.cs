using Newtonsoft.Json;
using PFM.Api.Contracts.ExpenseType;
using PFM.Api.Contracts.Shared;

namespace PFM.Services.ExternalServices.InMemoryStorage
{
    public class ExpenseTypeInMemory : IExpenseTypeApi
    {
        internal readonly IList<ExpenseTypeDetails> Storage = new List<ExpenseTypeDetails>()
        {
            new () { Id = 1, Name = "Groceries", GraphColor = "3399FF", ShowOnDashboard = true },
            new () { Id = 2, Name = "Energy", GraphColor = "33CC33", ShowOnDashboard = true },
            new () { Id = 3, Name = "Transport", GraphColor = "FF0000", ShowOnDashboard = true },
            new () { Id = 4, Name = "Healthcare & Wellbeing", GraphColor = "0066CC", ShowOnDashboard = true },
            new () { Id = 5, Name = "Leisure", GraphColor = "CCFFCC", ShowOnDashboard = true },
            new () { Id = 6, Name = "Telephone, TV, Internet", GraphColor = "E5CCFF", ShowOnDashboard = true },
            new () { Id = 7, Name = "Mortgage & Rent", GraphColor = "FF6666", ShowOnDashboard = true },
            new () { Id = 8, Name = "House", GraphColor = "FFCCE5", ShowOnDashboard = true },
            new () { Id = 9, Name = "Savings", GraphColor = "000000", ShowOnDashboard = false }
        };

        public async Task<ApiResponse> Create(string userId, ExpenseTypeDetails obj)
        {
            obj.Id = Storage.Max(x => x.Id) + 1;
            Storage.Add(obj);
            return await Task.FromResult(new ApiResponse(true));
        }

        public async Task<ApiResponse> Delete(int id)
        {
            var item = Storage.SingleOrDefault(x => x.Id == id);

            if (item == null)
                return await Task.FromResult(new ApiResponse(false));

            Storage.Remove(item);
            return await Task.FromResult(new ApiResponse(true));
        }

        public async Task<ApiResponse> Edit(int id, string userId, ExpenseTypeDetails obj)
        {
            var existing = Storage.SingleOrDefault(x => x.Id == id);

            if (existing == null)
                return await Task.FromResult(new ApiResponse(false));

            existing.Name = obj.Name;
            existing.GraphColor = obj.GraphColor;
            existing.ShowOnDashboard = obj.ShowOnDashboard;
            return await Task.FromResult(new ApiResponse(true));
        }

        public async Task<ApiResponse> Get(string userId)
        {
            var result = JsonConvert.SerializeObject(Storage.ToList());
            return await Task.FromResult(new ApiResponse((object)result));
        }

        public async Task<ApiResponse> Get(int id)
        {
            var item = JsonConvert.SerializeObject(Storage.SingleOrDefault(x => x.Id == id));
            return await Task.FromResult(new ApiResponse((object)item));
        }
    }
}

