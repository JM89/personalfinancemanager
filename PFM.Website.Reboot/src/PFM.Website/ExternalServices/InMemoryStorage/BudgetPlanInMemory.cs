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
            var budgetPlanExpenseType = _expenseTypes.Select(x => new BudgetPlanExpenseType()
            {
                ExpenseType = new ExpenseTypeList() { Id = x.Id, Name = x.Name },
                ExpectedValue = 0,
                PreviousMonthValue = 0,
                CurrentBudgetPlanValue = 0,
                AverageMonthValue = 0
            }).ToList();

            _storage = new List<BudgetPlanDetails>()
            {
                new BudgetPlanDetails()
                {
                    Id = 1,
                    Name = "Plan 2022",
                    StartDate = new DateTime(2022,01,01),
                    EndDate = new DateTime(2022,12,12),
                    ExpenseTypes = budgetPlanExpenseType
                },
                new BudgetPlanDetails()
                {
                    Id = 2,
                    Name = "Plan 2023 Q1",
                    StartDate = new DateTime(2023,01,01),
                    EndDate = new DateTime(2023,03,31),
                    ExpenseTypes = budgetPlanExpenseType
                },
                new BudgetPlanDetails()
                {
                    Id = 3,
                    Name = "Plan 2023 Q2",
                    StartDate = new DateTime(2023,04,01),
                    EndDate = new DateTime(2023,06,30),
                    ExpenseTypes = budgetPlanExpenseType
                },
                new BudgetPlanDetails()
                {
                    Id = 4,
                    Name = "Plan 2023 EOY",
                    PlannedStartDate = new DateTime(2023,07,01),
                    ExpenseTypes = budgetPlanExpenseType
                }
            };
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

