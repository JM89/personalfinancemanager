using PFM.Api.Contracts.MovementSummary;
using PFM.Api.Contracts.SearchParameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PFM.Services.Interfaces
{
    public interface IMovementSummaryService : IBaseService
    {
        Task<IList<MovementSummary>> GetMovementSummaryOverTime(MovementSummarySearchParameters search);
    }
}