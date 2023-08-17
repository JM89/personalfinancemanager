using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.Expense;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpGet("Get/{id}")]
        public async Task<ExpenseDetails> Get(int id)
        {
            return await _expenseService.GetById(id);
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
       
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _expenseService.DeleteExpense(id);
        }

        [HttpPost("CreateMultiple")]
        public async Task<bool> CreateMultiple([FromBody]List<ExpenseDetails> ExpenseDetails)
        {
            return await _expenseService.CreateExpenses(ExpenseDetails);
        }

        [HttpPost("ChangeDebitStatus/{id}/{debitStatus}")]
        public async Task<bool> ChangeDebitStatus(int id, bool debitStatus)
        {
            return await _expenseService.ChangeDebitStatus(id, debitStatus);
        }

        [HttpPost("GetExpenses")]
        public async Task<IList<ExpenseList>> GetExpenses([FromBody]Api.Contracts.SearchParameters.ExpenseGetListSearchParameters search)
        {
            return await _expenseService.GetExpenses(search);
        }
    }
}
