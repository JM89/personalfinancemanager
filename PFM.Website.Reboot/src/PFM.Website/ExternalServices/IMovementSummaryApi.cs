using Api.Contracts.Shared;
using PFM.Website.ExternalServices.Contracts;
using Refit;

namespace PFM.Website.ExternalServices
{
	public interface IMovementSummaryApi
	{
        [Post("/api/MovementSummary/GetList")]
        Task<ApiResponse> GetMovementSummaryOvertime(MovementSummarySearchParams searchParams);
    }
}

