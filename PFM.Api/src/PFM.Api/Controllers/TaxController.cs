using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.Tax;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaxController : ControllerBase
    {
        private readonly ITaxService _taxService;

        public TaxController(ITaxService TaxService)
        {
            _taxService = TaxService;
        }

        [HttpGet("GetList/{userId}")]
        public async Task<IEnumerable<TaxList>> GetList(string userId)
        {
            return await _taxService.GetTaxes(userId);
        }

        [HttpGet("GetTaxesByType/{userId}/{taxTypeId}")]
        public async Task<List<TaxList>> GetTaxesByType(string userId, int taxTypeId)
        {
            return await _taxService.GetTaxesByType(userId, taxTypeId);
        }

        [HttpGet("Get/{id}")]
        public async Task<TaxDetails> Get(int id)
        {
            return await _taxService.GetById(id);
        }
        
        [HttpPost("Create")]
        public async Task<bool> Post([FromBody]TaxDetails createdObj)
        {
            return await _taxService.CreateTax(createdObj);
        }
        
        [HttpPut("Edit/{id}")]
        public async Task<bool> Put(int id, [FromBody]TaxDetails editedObj)
        {
            editedObj.Id = id;
            return await _taxService.EditTax(editedObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _taxService.DeleteTax(id);
        }
    }
}
