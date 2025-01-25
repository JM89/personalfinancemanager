using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.MovementSummary;
using PFM.Services;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovementSummaryController(IMovementSummaryService service) : ControllerBase
    {
        [HttpPost("GetList")]
        public async Task<IList<MovementSummary>> GetMovementSummaryOverTime([FromBody] Api.Contracts.SearchParameters.MovementSummarySearchParameters search)
        {
            return await service.GetMovementSummaryOverTime(search);
        }
    }
}

