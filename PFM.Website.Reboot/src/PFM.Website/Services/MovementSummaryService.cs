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

