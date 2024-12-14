using AutoMapper;
using PFM.Website.Configurations;
using PFM.Website.ExternalServices;
using PFM.Api.Contracts.MovementSummary;
using PFM.Api.Contracts.SearchParameters;
using PFM.Website.Models;
using PFM.Website.Utils;

namespace PFM.Website.Services
{
    public class MovementSummaryService(
        Serilog.ILogger logger,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        ApplicationSettings settings,
        IMovementSummaryApi api)
        : CoreService(logger, mapper, httpContextAccessor, settings)
    {
        public async Task<DashboardExpenseTypeSummaryModel> GetExpenseTypeSummary(MovementSummarySearchParamModel search)
        {
            var request = Mapper.Map<MovementSummarySearchParameters>(search);
            var apiResponse = await api.GetMovementSummaryOvertime(request);
            var response = ReadApiResponse<List<MovementSummary>>(apiResponse) ?? new List<MovementSummary>();

            var expenseTypeSummaryModel = response
                .Where(x => x.Type == "Expenses")
                .GroupBy(x => x.Category)
                .ToDictionary(x => x.Key, y => y.ToList().Average(x => x.AggregatedAmount));

            return new DashboardExpenseTypeSummaryModel(expenseTypeSummaryModel);
        }

        public async Task<DashboardMovementTypeSummaryModel> GetMovementTypeSummary(MovementSummarySearchParamModel search)
        {
            var request = Mapper.Map<MovementSummarySearchParameters>(search);
            var apiResponse = await api.GetMovementSummaryOvertime(request);
            var response = ReadApiResponse<List<MovementSummary>>(apiResponse) ?? new List<MovementSummary>();

            var expenses = response.Where(x => x.Type == "Expenses");
            decimal averageExpensesOver12Months = expenses.Any() ? expenses.Average(x => x.AggregatedAmount) : 0;

            var incomes = response.Where(x => x.Type == "Incomes");
            decimal averageIncomesOver12Months = incomes.Any() ? incomes.Average(x => x.AggregatedAmount) : 0;

            var savings = response.Where(x => x.Type == "Savings");
            decimal averageSavingsOver12Months = savings.Any() ? savings.Average(x => x.AggregatedAmount) : 0;

            return new DashboardMovementTypeSummaryModel(
                response.Any() ? response.Last().AggregatedAmount : 0,
                averageExpensesOver12Months,
                averageIncomesOver12Months,
                averageSavingsOver12Months
            );
        }

        public async Task<DashboardMovementTypeOvertimeModel> GetMovementTypeOverTimeModel(MovementSummarySearchParamModel search)
        {
            var request = Mapper.Map<MovementSummarySearchParameters>(search);
            var apiResponse = await api.GetMovementSummaryOvertime(request);
            var response = ReadApiResponse<List<MovementSummary>>(apiResponse) ?? new List<MovementSummary>();

            var movementOverTimeModel = response
                .GroupBy(x => x.MonthYearIdentifier)
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, y => new MovementTypeOvertimeModel()
                {
                    ExpensesAmount = y.Where(x => x.Type == "Expenses").ToList().Sum(x => x.AggregatedAmount),
                    IncomesAmount = y.Where(x => x.Type == "Incomes").ToList().Sum(x => x.AggregatedAmount),
                    SavingsAmount = y.Where(x => x.Type == "Savings").ToList().Sum(x => x.AggregatedAmount)
                });

            return new DashboardMovementTypeOvertimeModel(movementOverTimeModel);
        }

        public async Task<DashboardExpenseOvertimeModel> GetExpenseOvertime(MovementSummarySearchParamModel search)
        {
            var request = Mapper.Map<MovementSummarySearchParameters>(search);
            var apiResponse = await api.GetMovementSummaryOvertime(request);
            var response = ReadApiResponse<List<MovementSummary>>(apiResponse) ?? new List<MovementSummary>();

            var expenseOvertimeModel = response
                .GroupBy(x => x.MonthYearIdentifier)
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, y => new ExpenseOvertimeModel()
                {
                    Actual = y.Where(x => x.Type == "Expenses").ToList().Sum(x => x.AggregatedAmount)
                });

            return new DashboardExpenseOvertimeModel(expenseOvertimeModel);
        }

        public async Task<DashboardExpenseTypeOvertimeModel> GetExpenseTypeOvertime(MovementSummarySearchParamModel search)
        {
            var request = Mapper.Map<MovementSummarySearchParameters>(search);
            var apiResponse = await api.GetMovementSummaryOvertime(request);
            var response = ReadApiResponse<List<MovementSummary>>(apiResponse) ?? new List<MovementSummary>();
            var data = response
                .GroupBy(x => x.MonthYearIdentifier)
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, y => y.ToList().Sum(x => x.AggregatedAmount));
            return new DashboardExpenseTypeOvertimeModel(data);
        }

        public async Task<DashboardExpenseTypeDiffsModel> GetExpenseTypePaged(int skip, int take, MovementSummarySearchParamModel search)
        {
            if (search.OptionalType == null)
            {
                throw new InvalidOperationException($"{nameof(search.OptionalType)} must be set.");
            }

            var request = Mapper.Map<MovementSummarySearchParameters>(search);
            var apiResponse = await api.GetMovementSummaryOvertime(request);
            var response = ReadApiResponse<List<MovementSummary>>(apiResponse) ?? new List<MovementSummary>();

            var previousMonthIdentifier = MonthYearHelper.ConvertToYYYYMM(DateTime.UtcNow.AddMonths(-1));

            var responseByCategories = response.GroupBy(x => x.Category);
            var pagedByCategories = responseByCategories.Skip(skip).Take(take);

            var data = pagedByCategories
                .Select(x => new ExpenseTypeDiffsModel() {
                    ExpenseTypeName = x.Key,
                    Actual = x.Any() ? x.Last().AggregatedAmount : 0,
                    Expected = 0, // Budget
                    PreviousMonth = x.Any(y => y.MonthYearIdentifier == previousMonthIdentifier) ? x.Where(y => y.MonthYearIdentifier == previousMonthIdentifier).Sum(x => x.AggregatedAmount) : 0,
                    Average = x.Any() ? x.Average(x => x.AggregatedAmount) : 0
                }).ToList();

            return new DashboardExpenseTypeDiffsModel(new PagedModel<ExpenseTypeDiffsModel>(data, responseByCategories.Count()));
        }
    }
}

