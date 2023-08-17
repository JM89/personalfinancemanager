using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.MovementSummary;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovementSummaryController : ControllerBase
    {
        private readonly IMovementSummaryService _movementSummaryService;

        public MovementSummaryController(IMovementSummaryService movementSummaryService)
        {
            _movementSummaryService = movementSummaryService;
        }


        [HttpPost("GetList")]
        public async Task<IList<MovementSummary>> GetMovementSummaryOverTime([FromBody] Api.Contracts.SearchParameters.MovementSummarySearchParameters search)
        {
            return await _movementSummaryService.GetMovementSummaryOverTime(search);
        }
    }
}

