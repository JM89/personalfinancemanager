using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.Pension;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PensionController : ControllerBase
    {
        private readonly IPensionService _pensionService;

        public PensionController(IPensionService PensionService)
        {
            _pensionService = PensionService;
        }

        [HttpGet("GetList/{userId}")]
        public async Task<IEnumerable<PensionList>> GetList(string userId)
        {
            return await _pensionService.GetPensions(userId);
        }

        [HttpGet("Get/{id}")]
        public Task<PensionDetails> Get(int id)
        {
            return _pensionService.GetById(id);
        }
        
        [HttpPost("Create")]
        public Task<bool> Post([FromBody]PensionDetails createdObj)
        {
            return _pensionService.CreatePension(createdObj);
        }
        
        [HttpPut("Edit/{id}")]
        public Task<bool> Put(int id, [FromBody]PensionDetails editedObj)
        {
            editedObj.Id = id;
            return _pensionService.EditPension(editedObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public Task<bool> Delete(int id)
        {
            return _pensionService.DeletePension(id);
        }
    }
}
