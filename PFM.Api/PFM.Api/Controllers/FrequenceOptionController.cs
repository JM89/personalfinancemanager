using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PFM.DTOs.FrequenceOption;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/FrequenceOption")]
    public class FrequenceOptionController : Controller
    {
        private readonly IFrequenceOptionService _FrequenceOptionService;

        public FrequenceOptionController(IFrequenceOptionService FrequenceOptionService)
        {
            _FrequenceOptionService = FrequenceOptionService;
        }

        [HttpGet("GetList")]
        public IEnumerable<FrequenceOptionList> GetList()
        {
            return _FrequenceOptionService.GetFrequencyOptions();
        }
    }
}
