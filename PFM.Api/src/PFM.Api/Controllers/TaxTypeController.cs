using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.TaxType;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/TaxType")]
    public class TaxTypeController : Controller
    {
        private readonly ITaxTypeService _TaxTypeService;

        public TaxTypeController(ITaxTypeService TaxTypeService)
        {
            _TaxTypeService = TaxTypeService;
        }

        [HttpGet("GetList")]
        public IEnumerable<TaxTypeList> GetList()
        {
            return _TaxTypeService.GetTaxTypes();
        }
    }
}
