using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.BudgetPlan;
using PFM.Api.Contracts.Dashboard;
using PFM.Api.Contracts.Expense;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService ExpenseService)
        {
            _expenseService = ExpenseService;
        }

        [HttpGet("Get/{id}")]
        public ExpenseDetails Get(int id)
        {
            return _expenseService.GetById(id);
        }
        
        [HttpPost("Create")]
        public async Task<bool> Post([FromBody]ExpenseDetails createdObj)
        {
            return await _expenseService.CreateExpense(createdObj);
        }

        [HttpPost("CreateExpenses")]
        public async Task<bool> CreateExpenses([FromBody]List<ExpenseDetails> createdObjs)
        {
            return await _expenseService.CreateExpenses(createdObjs);
        }

        [HttpPut("Edit/{id}")]
        public async Task<bool> Put(int id, [FromBody]ExpenseDetails editedObj)
        {
            return await _expenseService.EditExpense(editedObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _expenseService.DeleteExpense(id);
        }

        [HttpPost("CreateMultiple")]
        public void CreateMultiple([FromBody]List<ExpenseDetails> ExpenseDetails)
        {
            _expenseService.CreateExpenses(ExpenseDetails);
        }

        [HttpPost("ChangeDebitStatus/{id}/{debitStatus}")]
        public void ChangeDebitStatus(int id, bool debitStatus)
        {
            _expenseService.ChangeDebitStatus(id, debitStatus);
        }

        [HttpPost("GetExpenseSummary/{accountId}")]
        public ExpenseSummary GetExpenseSummary(int accountId, [FromBody]BudgetPlanDetails budgetPlan)
        {
            return _expenseService.GetExpenseSummary(accountId, budgetPlan);
        }

        [HttpPost("GetExpenses")]
        public IList<ExpenseList> GetExpenses([FromBody]Api.Contracts.SearchParameters.ExpenseGetListSearchParameters search)
        {
            return _expenseService.GetExpenses(search);
        }
    }
}
