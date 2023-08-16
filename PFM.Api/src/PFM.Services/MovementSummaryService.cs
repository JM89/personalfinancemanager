using AutoMapper;
using PFM.Api.Contracts.MovementSummary;
using PFM.DataAccessLayer.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFM.Services.Interfaces.Services
{
    public class MovementSummaryService : IMovementSummaryService
    {
        private readonly IMovementSummaryRepository _movementSummaryRepository;

        public MovementSummaryService(IMovementSummaryRepository movementSummaryRepository)
        {
            _movementSummaryRepository = movementSummaryRepository;
        }

        public async Task<IList<MovementSummary>> GetMovementSummaryOverTime(Api.Contracts.SearchParameters.MovementSummarySearchParameters search)
        {
            var searchParams = Mapper.Map<PFM.DataAccessLayer.SearchParameters.MovementSummarySearchParameters>(search);
            var entities = await _movementSummaryRepository.GetMovementSummaryOverTime(searchParams);
            return entities.Select(Mapper.Map<MovementSummary>).ToList();
        }
    }
}