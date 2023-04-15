using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.Tax;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Tax")]
    public class TaxController : Controller
    {
        private readonly ITaxService _TaxService;

        public TaxController(ITaxService TaxService)
        {
            _TaxService = TaxService;
        }

        [HttpGet("GetList/{userId}")]
        public IEnumerable<TaxList> GetList(string userId)
        {
            return _TaxService.GetTaxes(userId);
        }

        [HttpGet("GetTaxesByType/{userId}/{taxTypeId}")]
        public IList<TaxList> GetTaxesByType(string userId, int taxTypeId)
        {
            return _TaxService.GetTaxesByType(userId, taxTypeId);
        }

        [HttpGet("Get/{id}")]
        public TaxDetails Get(int id)
        {
            return _TaxService.GetById(id);
        }
        
        [HttpPost("Create")]
        public void Post([FromBody]TaxDetails createdObj)
        {
            _TaxService.CreateTax(createdObj);
        }
        
        [HttpPut("Edit/{id}")]
        public void Put(int id, [FromBody]TaxDetails editedObj)
        {
            _TaxService.EditTax(editedObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public void Delete(int id)
        {
            _TaxService.DeleteTax(id);
        }
    }
}
