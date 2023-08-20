using Api.Contracts.Shared;
using Newtonsoft.Json;
using PFM.Api.Contracts.BudgetPlan;
using PFM.Api.Contracts.ExpenseType;

namespace PFM.Website.ExternalServices.InMemoryStorage
{
    public class BudgetPlanInMemory : IBudgetPlanApi
    {
        internal IList<BudgetPlanDetails> _storage;
        private IList<ExpenseTypeDetails> _expenseTypes = new ExpenseTypeInMemory()._storage.ToList();

        public BudgetPlanInMemory()
        {
            var today = DateTime.UtcNow;
            var yearsBack = new DateTime(today.Year-4, 1, 1);

            _storage = new List<BudgetPlanDetails>();

            var random = new Random();
            for (int i = 1; i <= 5; i++)
            {
                var budgetPlanExpenseType = _expenseTypes.Select(x => new BudgetPlanExpenseType()
                {
                    ExpenseType = new ExpenseTypeList() { Id = x.Id, Name = x.Name },
                    ExpectedValue = random.Next(50, 300),
                }).ToList();


                var budgetPlan = new BudgetPlanDetails()
                {
                    Id = i,
                    Name = $"Plan {yearsBack.Year}",
                    StartDate = i != 5 ? yearsBack : null,
                    EndDate = i != 5 ? yearsBack.AddYears(1).AddSeconds(-1) : null,
                    PlannedStartDate = i == 5 ? yearsBack : null,
                    ExpenseTypes = budgetPlanExpenseType,
                    ExpectedIncomes = 2500,
                    ExpectedSavings = 1500
                };

                yearsBack = yearsBack.AddYears(1);
                _storage.Add(budgetPlan);
            }
        }

        public async Task<ApiResponse> Create(int accountId, BudgetPlanDetails obj)
        {
            obj.Id = _storage.Max(x => x.Id) + 1;
            _storage.Add(obj);
            return await Task.FromResult(new ApiResponse(true));
        }

        public async Task<ApiResponse> Edit(int accountId, BudgetPlanDetails obj)
        {
            var existing = _storage.SingleOrDefault(x => x.Id == obj.Id);

            if (existing == null)
                return await Task.FromResult(new ApiResponse(false));

            existing.Name = obj.Name;
            existing.PlannedStartDate = obj.PlannedStartDate;
            existing.ExpenseTypes = obj.ExpenseTypes;

            return await Task.FromResult(new ApiResponse(true));
        }

        public async Task<ApiResponse> Get(int id)
        {
            var item = JsonConvert.SerializeObject(_storage.SingleOrDefault(x => x.Id == id));
            return await Task.FromResult(new ApiResponse((object)item));
        }

        public async Task<ApiResponse> GetList(int accountId)
        {
            var result = JsonConvert.SerializeObject(_storage.ToList());
            return await Task.FromResult(new ApiResponse((object)result));
        }

        public async Task<ApiResponse> Start(int value, int accountId)
        {
            var existing = _storage.SingleOrDefault(x => x.Id == value);

            if (existing == null)
                return await Task.FromResult(new ApiResponse(false));

            existing.StartDate = DateTime.UtcNow;

            return await Task.FromResult(new ApiResponse(true));
        }

        public async Task<ApiResponse> Stop(int value)
        {
            var existing = _storage.SingleOrDefault(x => x.Id == value);

            if (existing == null)
                return await Task.FromResult(new ApiResponse(false));

            existing.EndDate = DateTime.UtcNow;

            return await Task.FromResult(new ApiResponse(true));
        }
    }
}

