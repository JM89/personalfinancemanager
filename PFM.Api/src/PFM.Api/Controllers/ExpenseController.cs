using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.Expense;
using PFM.Services;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController(IExpenseService api) : ControllerBase
    {
        [HttpGet("Get/{id}")]
        public async Task<ExpenseDetails> Get(int id)
        {
            return await api.GetById(id);
        }
        
        [HttpPost("Create")]
        public async Task<bool> Post([FromBody]ExpenseDetails createdObj)
        {
            return await api.CreateExpense(createdObj);
        }

        [HttpPost("CreateExpenses")]
        public async Task<bool> CreateExpenses([FromBody]List<ExpenseDetails> createdObjs)
        {
            return await api.CreateExpenses(createdObjs);
        }
       
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await api.DeleteExpense(id);
        }

        [HttpPost("CreateMultiple")]
        public async Task<bool> CreateMultiple([FromBody]List<ExpenseDetails> ExpenseDetails)
        {
            return await api.CreateExpenses(ExpenseDetails);
        }

        [HttpPost("ChangeDebitStatus/{id}/{debitStatus}")]
        public async Task<bool> ChangeDebitStatus(int id, bool debitStatus)
        {
            return await api.ChangeDebitStatus(id, debitStatus);
        }

        [HttpPost("GetExpenses")]
        public async Task<IList<ExpenseList>> GetExpenses([FromBody]Api.Contracts.SearchParameters.ExpenseGetListSearchParameters search)
        {
            return await api.GetExpenses(search);
        }
    }
}
