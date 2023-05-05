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
        public async Task<PensionDetails> Get(int id)
        {
            return await _pensionService.GetById(id);
        }
        
        [HttpPost("Create")]
        public async Task<bool> Post([FromBody]PensionDetails createdObj)
        {
            return await _pensionService.CreatePension(createdObj);
        }
        
        [HttpPut("Edit/{id}")]
        public async Task<bool> Put(int id, [FromBody]PensionDetails editedObj)
        {
            editedObj.Id = id;
            return await _pensionService.EditPension(editedObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _pensionService.DeletePension(id);
        }
    }
}
