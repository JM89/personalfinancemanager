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
        private readonly IBudgetPlanService _BudgetPlanService;

        public BudgetPlanController(IBudgetPlanService BudgetPlanService)
        {
            _BudgetPlanService = BudgetPlanService;
        }

        [HttpGet("GetList/{accountId}")]
        public IEnumerable<BudgetPlanList> GetList(int accountId)
        {
            return _BudgetPlanService.GetBudgetPlans(accountId);
        }

        [HttpGet("Get/{id}")]
        public BudgetPlanDetails Get(int id)
        {
            return _BudgetPlanService.GetById(id);
        }

        [HttpGet("GetCurrent/{accountId}")]
        public BudgetPlanDetails GetCurrent(int accountId)
        {
            return _BudgetPlanService.GetCurrent(accountId);
        }

        [HttpPost("Create/{accountId}")]
        public void Create(int accountId, [FromBody]BudgetPlanDetails budgetPlanDetails)
        {
            _BudgetPlanService.CreateBudgetPlan(budgetPlanDetails, accountId);
        }

        [HttpPut("Edit/{accountId}")]
        public void Edit(int accountId, [FromBody]BudgetPlanDetails budgetPlanDetails)
        {
            _BudgetPlanService.EditBudgetPlan(budgetPlanDetails, accountId);
        }

        [HttpPost("Start/{value}/{accountId}")]
        public void Start(int value, int accountId)
        {
            _BudgetPlanService.StartBudgetPlan(value, accountId);
        }

        [HttpPost("Stop/{value}")]
        public void Stop(int value)
        {
            _BudgetPlanService.StopBudgetPlan(value);
        }

        [HttpGet("BuildEmpty/{accountId}/{budgetPlanId?}")]
        public BudgetPlanDetails Build(int accountId, int? budgetPlanId = null)
        {
            return _BudgetPlanService.BuildBudgetPlan(accountId, budgetPlanId);
        }
    }
}
