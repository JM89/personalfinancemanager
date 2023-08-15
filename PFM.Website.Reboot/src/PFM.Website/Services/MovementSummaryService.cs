using AutoMapper;
using PFM.Website.Configurations;
using PFM.Website.ExternalServices;
using PFM.Website.ExternalServices.Contracts;
using PFM.Website.Models;

namespace PFM.Website.Services
{
    public class MovementSummaryService : CoreService
    {
        private readonly IMovementSummaryApi _api;

        public MovementSummaryService(Serilog.ILogger logger, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            ApplicationSettings settings, IMovementSummaryApi api)
            : base(logger, mapper, httpContextAccessor, settings)
        {
            _api = api;
        }

        public async Task<DashboardExpenseTypeSummaryModel> GetExpenseTypeSummary(MovementSummarySearchParamModel search)
        {
            var request = _mapper.Map<MovementSummarySearchParams>(search);
            var apiResponse = await _api.GetMovementSummaryOvertime(request);
            var response = ReadApiResponse<List<MovementSummary>>(apiResponse) ?? new List<MovementSummary>();

            var expenseTypeSummaryModel = response
                .Where(x => x.Type == "Expenses")
                .GroupBy(x => x.Category)
                .ToDictionary(x => x.Key, y => y.ToList().Average(x => x.AggregatedAmount));

            return new DashboardExpenseTypeSummaryModel(expenseTypeSummaryModel);
        }

        public async Task<DashboardMovementTypeSummaryModel> GetMovementTypeSummary(MovementSummarySearchParamModel search)
        {
            var request = _mapper.Map<MovementSummarySearchParams>(search);
            var apiResponse = await _api.GetMovementSummaryOvertime(request);
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
            var request = _mapper.Map<MovementSummarySearchParams>(search);
            var apiResponse = await _api.GetMovementSummaryOvertime(request);
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
            var request = _mapper.Map<MovementSummarySearchParams>(search);
            var apiResponse = await _api.GetMovementSummaryOvertime(request);
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
            var request = _mapper.Map<MovementSummarySearchParams>(search);
            var apiResponse = await _api.GetMovementSummaryOvertime(request);
            var response = ReadApiResponse<List<MovementSummary>>(apiResponse) ?? new List<MovementSummary>();
            var data = response
                .GroupBy(x => x.MonthYearIdentifier)
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, y => y.ToList().Sum(x => x.AggregatedAmount));
            return new DashboardExpenseTypeOvertimeModel(data);
        }
    }
}

