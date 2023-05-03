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
        public Task<List<TaxList>> GetTaxesByType(string userId, int taxTypeId)
        {
            return _taxService.GetTaxesByType(userId, taxTypeId);
        }

        [HttpGet("Get/{id}")]
        public Task<TaxDetails> Get(int id)
        {
            return _taxService.GetById(id);
        }
        
        [HttpPost("Create")]
        public Task<bool> Post([FromBody]TaxDetails createdObj)
        {
            return _taxService.CreateTax(createdObj);
        }
        
        [HttpPut("Edit/{id}")]
        public Task<bool> Put(int id, [FromBody]TaxDetails editedObj)
        {
            editedObj.Id = id;
            return _taxService.EditTax(editedObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public Task<bool> Delete(int id)
        {
            return _taxService.DeleteTax(id);
        }
    }
}
