using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.Income;
using PFM.Services;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncomeController(IIncomeService api) : ControllerBase
    {
        [HttpGet("GetList/{accountId}")]
        public async Task<IEnumerable<IncomeList>> GetList(int accountId)
        {
            return await api.GetIncomes(accountId);
        }

        [HttpGet("Get/{id}")]
        public async Task<IncomeDetails> Get(int id)
        {
            return await api.GetById(id);
        }
        
        [HttpPost("Create")]
        public async Task<bool> Post([FromBody]IncomeDetails createdObj)
        {
            return await api.CreateIncome(createdObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await api.DeleteIncome(id);
        }
        
        [HttpPost("CreateIncomes")]
        public async Task<bool> CreateIncomes([FromBody]List<IncomeDetails> createdObjs)
        {
            return await api.CreateIncomes(createdObjs);
        }
    }
}
