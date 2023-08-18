using Api.Contracts.Shared;
using PFM.Api.Contracts.SearchParameters;
using Refit;

namespace PFM.Website.ExternalServices
{
	public interface IMovementSummaryApi
	{
        [Post("/api/MovementSummary/GetList")]
        Task<ApiResponse> GetMovementSummaryOvertime(MovementSummarySearchParameters searchParams);
    }
}

