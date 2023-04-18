using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.FrequenceOption;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FrequenceOptionController : ControllerBase
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
