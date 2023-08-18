using Api.Contracts.Shared;
using Newtonsoft.Json;
using PFM.Api.Contracts.ExpenseType;
using PFM.Website.Utils;
using PFM.Api.Contracts.MovementSummary;
using PFM.Api.Contracts.SearchParameters;

namespace PFM.Website.ExternalServices.InMemoryStorage
{
    public class MovementSummaryInMemory : IMovementSummaryApi
    {
        internal IList<MovementSummary> _storage;
        private IList<ExpenseTypeDetails> _expenseTypes = new ExpenseTypeInMemory()._storage.ToList();
        
        public MovementSummaryInMemory()
        {
            var historicData = MonthYearHelper.GetXLastMonths(24, true, false);
            var rng = new Random();
            _storage = new List<MovementSummary>();

            foreach (var monthYear in historicData)
            {
                foreach (var expenseType in _expenseTypes)
                {
                    var summary = new MovementSummary()
                    {
                        Category = expenseType.Name,
                        MonthYearIdentifier = monthYear,
                        BankAccountId = 1,
                        AggregatedAmount = rng.Next(100, 1000),
                        Type = "Expenses"
                    };
                    _storage.Add(summary);
                }
            }

            foreach (var monthYear in historicData)
            {
                var summary = new MovementSummary()
                {
                    MonthYearIdentifier = monthYear,
                    BankAccountId = 1,
                    AggregatedAmount = rng.Next(100, 1000),
                    Type = "Savings"
                };
                _storage.Add(summary);
            }
            foreach (var monthYear in historicData)
            {
                var summary = new MovementSummary()
                {
                    MonthYearIdentifier = monthYear,
                    BankAccountId = 1,
                    AggregatedAmount = rng.Next(100, 1000),
                    Type = "Incomes"
                };
                _storage.Add(summary);
            }
        }

        public async Task<ApiResponse> GetMovementSummaryOvertime(MovementSummarySearchParameters search)
        {
            var data = _storage.Where(x => x.BankAccountId == search.BankAccountId && search.MonthYearIdentifiers.Contains(x.MonthYearIdentifier));

            if (!string.IsNullOrEmpty(search.OptionalCategory))
            {
                data = data.Where(x => x.Category == search.OptionalCategory);
            }

            if (!string.IsNullOrEmpty(search.OptionalType))
            {
                data = data.Where(x => x.Type == search.OptionalType);
            }

            if (search.ExcludedCategories != null && search.ExcludedCategories.Any())
            {
                data = data.Where(x => !search.ExcludedCategories.Contains(x.Category));
            }

            var result = JsonConvert.SerializeObject(data.ToList());

            return await Task.FromResult(new ApiResponse((object)result));
        }
    }
}

