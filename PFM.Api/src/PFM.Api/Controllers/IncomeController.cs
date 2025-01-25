using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.Income;
using PFM.Services;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncomeController(IIncomeService service) : ControllerBase
    {
        [HttpGet("GetList/{accountId}")]
        public async Task<IEnumerable<IncomeList>> GetList(int accountId)
        {
            return await service.GetIncomes(accountId);
        }

        [HttpGet("Get/{id}")]
        public async Task<IncomeDetails> Get(int id)
        {
            return await service.GetById(id);
        }
        
        [HttpPost("Create")]
        public async Task<bool> Post([FromBody]IncomeDetails createdObj)
        {
            return await service.CreateIncome(createdObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await service.DeleteIncome(id);
        }
        
        [HttpPost("CreateIncomes")]
        public async Task<bool> CreateIncomes([FromBody]List<IncomeDetails> createdObjs)
        {
            return await service.CreateIncomes(createdObjs);
        }
    }
}
