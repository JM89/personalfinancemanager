using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.BudgetPlan;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BudgetPlanController : ControllerBase
    {
        private readonly IBudgetPlanService _budgetPlanService;

        public BudgetPlanController(IBudgetPlanService BudgetPlanService)
        {
            _budgetPlanService = BudgetPlanService;
        }

        [HttpGet("GetList/{accountId}")]
        public IEnumerable<BudgetPlanList> GetList(int accountId)
        {
            return _budgetPlanService.GetBudgetPlans(accountId);
        }

        [HttpGet("Get/{id}")]
        public BudgetPlanDetails Get(int id)
        {
            return _budgetPlanService.GetById(id);
        }

        [HttpGet("GetCurrent/{accountId}")]
        public IActionResult GetCurrent(int accountId)
        {
            var current = _budgetPlanService.GetCurrent(accountId);
            return current != null ? Ok(current) : Ok();
        }

        [HttpPost("Create/{accountId}")]
        public void Create(int accountId, [FromBody]BudgetPlanDetails budgetPlanDetails)
        {
            _budgetPlanService.CreateBudgetPlan(budgetPlanDetails, accountId);
        }

        [HttpPut("Edit/{accountId}")]
        public void Edit(int accountId, [FromBody]BudgetPlanDetails budgetPlanDetails)
        {
            _budgetPlanService.EditBudgetPlan(budgetPlanDetails, accountId);
        }

        [HttpPost("Start/{value}/{accountId}")]
        public void Start(int value, int accountId)
        {
            _budgetPlanService.StartBudgetPlan(value, accountId);
        }

        [HttpPost("Stop/{value}")]
        public void Stop(int value)
        {
            _budgetPlanService.StopBudgetPlan(value);
        }

        [HttpGet("BuildEmpty/{accountId}/{budgetPlanId?}")]
        public async Task<BudgetPlanDetails> Build(int accountId, int? budgetPlanId = null)
        {
            return await _budgetPlanService.BuildBudgetPlan(accountId, budgetPlanId);
        }
    }
}
