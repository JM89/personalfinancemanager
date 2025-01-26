using PFM.Api.Contracts.SearchParameters;
using PFM.Api.Contracts.Shared;
using Refit;

namespace PFM.Services.ExternalServices
{
	public interface IMovementSummaryApi
	{
        [Post("/api/MovementSummary/GetList")]
        Task<ApiResponse> GetMovementSummaryOvertime(MovementSummarySearchParameters searchParams);
    }
}

