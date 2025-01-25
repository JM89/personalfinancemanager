using AutoMapper;
using PFM.Api.Contracts.MovementSummary;
using PFM.DataAccessLayer.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PFM.Api.Contracts.SearchParameters;
using PFM.Services.Core;

namespace PFM.Services;

public interface IMovementSummaryService : IBaseService
{
    Task<IList<MovementSummary>> GetMovementSummaryOverTime(MovementSummarySearchParameters search);
}

public class MovementSummaryService(IMapper mapper, IMovementSummaryRepository movementSummaryRepository) : IMovementSummaryService
{
    public async Task<IList<MovementSummary>> GetMovementSummaryOverTime(Api.Contracts.SearchParameters.MovementSummarySearchParameters search)
    {
        var searchParams = mapper.Map<PFM.DataAccessLayer.SearchParameters.MovementSummarySearchParameters>(search);
        var entities = await movementSummaryRepository.GetMovementSummaryOverTime(searchParams);
        return entities.Select(mapper.Map<MovementSummary>).ToList();
    }
}