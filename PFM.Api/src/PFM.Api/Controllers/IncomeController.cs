using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.Income;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeService _incomeService;

        public IncomeController(IIncomeService IncomeService)
        {
            _incomeService = IncomeService;
        }

        [HttpGet("GetList/{accountId}")]
        public IEnumerable<IncomeList> GetList(int accountId)
        {
            return _incomeService.GetIncomes(accountId);
        }

        [HttpGet("Get/{id}")]
        public IncomeDetails Get(int id)
        {
            return _incomeService.GetById(id);
        }
        
        [HttpPost("Create")]
        public async Task<bool> Post([FromBody]IncomeDetails createdObj)
        {
            return await _incomeService.CreateIncome(createdObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _incomeService.DeleteIncome(id);
        }
        
        [HttpPost("CreateIncomes")]
        public async Task<bool> CreateIncomes([FromBody]List<IncomeDetails> createdObjs)
        {
            return await _incomeService.CreateIncomes(createdObjs);
        }
    }
}
