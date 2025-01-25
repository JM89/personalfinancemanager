using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.Expense;
using PFM.Services;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController(IExpenseService service) : ControllerBase
    {
        [HttpGet("Get/{id}")]
        public async Task<ExpenseDetails> Get(int id)
        {
            return await service.GetById(id);
        }
        
        [HttpPost("Create")]
        public async Task<bool> Post([FromBody]ExpenseDetails createdObj)
        {
            return await service.CreateExpense(createdObj);
        }

        [HttpPost("CreateExpenses")]
        public async Task<bool> CreateExpenses([FromBody]List<ExpenseDetails> createdObjs)
        {
            return await service.CreateExpenses(createdObjs);
        }
       
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await service.DeleteExpense(id);
        }

        [HttpPost("CreateMultiple")]
        public async Task<bool> CreateMultiple([FromBody]List<ExpenseDetails> ExpenseDetails)
        {
            return await service.CreateExpenses(ExpenseDetails);
        }

        [HttpPost("ChangeDebitStatus/{id}/{debitStatus}")]
        public async Task<bool> ChangeDebitStatus(int id, bool debitStatus)
        {
            return await service.ChangeDebitStatus(id, debitStatus);
        }

        [HttpPost("GetExpenses")]
        public async Task<IList<ExpenseList>> GetExpenses([FromBody]Api.Contracts.SearchParameters.ExpenseGetListSearchParameters search)
        {
            return await service.GetExpenses(search);
        }
    }
}
