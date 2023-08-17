using System.Collections.Generic;
using System.Threading.Tasks;

namespace PFM.DataAccessLayer.Repositories.Interfaces
{
	public interface IMovementSummaryRepository
	{
        Task<IEnumerable<Entities.MovementSummary>> GetMovementSummaryOverTime(SearchParameters.MovementSummarySearchParameters search);
    }
}

