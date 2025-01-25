using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.BudgetPlan;
using PFM.Services;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BudgetPlanController(IBudgetPlanService service) : ControllerBase
    {
        [HttpGet("GetList/{accountId}")]
        public async Task<IEnumerable<BudgetPlanList>> GetList(int accountId)
        {
            return await service.GetBudgetPlans(accountId);
        }

        [HttpGet("Get/{id}")]
        public async Task<BudgetPlanDetails> Get(int id)
        {
            return await service.GetById(id);
        }

        [HttpGet("GetCurrent/{accountId}")]
        public async Task<IActionResult> GetCurrent(int accountId)
        {
            var current = await service.GetCurrent(accountId);
            return current != null ? Ok(current) : Ok();
        }

        [HttpPost("Create/{accountId}")]
        public async Task<bool> Create(int accountId, [FromBody]BudgetPlanDetails budgetPlanDetails)
        {
            return await service.CreateBudgetPlan(budgetPlanDetails, accountId);
        }

        [HttpPut("Edit/{accountId}")]
        public async Task<bool> Edit(int accountId, [FromBody]BudgetPlanDetails budgetPlanDetails)
        {
            return await service.EditBudgetPlan(budgetPlanDetails, accountId);
        }

        [HttpPost("Start/{value}/{accountId}")]
        public async Task<bool> Start(int value, int accountId)
        {
            return await service.StartBudgetPlan(value, accountId);
        }

        [HttpPost("Stop/{value}")]
        public async Task<bool> Stop(int value)
        {
            return await service.StopBudgetPlan(value);
        }
    }
}
