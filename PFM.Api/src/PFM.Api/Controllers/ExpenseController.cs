using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.BudgetPlan;
using PFM.Api.Contracts.Dashboard;
using PFM.Api.Contracts.Expense;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Expense")]
    public class ExpenseController : Controller
    {
        private readonly IExpenseService _ExpenseService;

        public ExpenseController(IExpenseService ExpenseService)
        {
            _ExpenseService = ExpenseService;
        }

        [HttpGet("Get/{id}")]
        public ExpenseDetails Get(int id)
        {
            return _ExpenseService.GetById(id);
        }
        
        [HttpPost("Create")]
        public void Post([FromBody]ExpenseDetails createdObj)
        {
            _ExpenseService.CreateExpense(createdObj);
        }

        [HttpPost("CreateExpenses")]
        public void CreateExpenses([FromBody]List<ExpenseDetails> createdObjs)
        {
            _ExpenseService.CreateExpenses(createdObjs);
        }

        [HttpPut("Edit/{id}")]
        public void Put(int id, [FromBody]ExpenseDetails editedObj)
        {
            _ExpenseService.EditExpense(editedObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public void Delete(int id)
        {
            _ExpenseService.DeleteExpense(id);
        }

        [HttpPost("CreateMultiple")]
        public void CreateMultiple([FromBody]List<ExpenseDetails> ExpenseDetails)
        {
            _ExpenseService.CreateExpenses(ExpenseDetails);
        }

        [HttpPost("ChangeDebitStatus/{id}/{debitStatus}")]
        public void ChangeDebitStatus(int id, bool debitStatus)
        {
            _ExpenseService.ChangeDebitStatus(id, debitStatus);
        }

        [HttpPost("GetExpenseSummary/{accountId}")]
        public ExpenseSummary GetExpenseSummary(int accountId, [FromBody]BudgetPlanDetails budgetPlan)
        {
            return _ExpenseService.GetExpenseSummary(accountId, budgetPlan);
        }

        [HttpPost("GetExpenses")]
        public IList<ExpenseList> GetExpenses([FromBody]Api.Contracts.SearchParameters.ExpenseGetListSearchParameters search)
        {
            return _ExpenseService.GetExpenses(search);
        }
    }
}
