using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.BudgetPlan;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BudgetPlanController(IBudgetPlanService budgetPlanService) : ControllerBase
    {
        [HttpGet("GetList/{accountId}")]
        public async Task<IEnumerable<BudgetPlanList>> GetList(int accountId)
        {
            return await budgetPlanService.GetBudgetPlans(accountId);
        }

        [HttpGet("Get/{id}")]
        public async Task<BudgetPlanDetails> Get(int id)
        {
            return await budgetPlanService.GetById(id);
        }

        [HttpGet("GetCurrent/{accountId}")]
        public async Task<IActionResult> GetCurrent(int accountId)
        {
            var current = await budgetPlanService.GetCurrent(accountId);
            return current != null ? Ok(current) : Ok();
        }

        [HttpPost("Create/{accountId}")]
        public async Task<bool> Create(int accountId, [FromBody]BudgetPlanDetails budgetPlanDetails)
        {
            return await budgetPlanService.CreateBudgetPlan(budgetPlanDetails, accountId);
        }

        [HttpPut("Edit/{accountId}")]
        public async Task<bool> Edit(int accountId, [FromBody]BudgetPlanDetails budgetPlanDetails)
        {
            return await budgetPlanService.EditBudgetPlan(budgetPlanDetails, accountId);
        }

        [HttpPost("Start/{value}/{accountId}")]
        public async Task<bool> Start(int value, int accountId)
        {
            return await budgetPlanService.StartBudgetPlan(value, accountId);
        }

        [HttpPost("Stop/{value}")]
        public async Task<bool> Stop(int value)
        {
            return await budgetPlanService.StopBudgetPlan(value);
        }
    }
}
