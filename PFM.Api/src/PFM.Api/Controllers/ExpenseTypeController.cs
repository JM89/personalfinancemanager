using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.ExpenseType;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseTypeController : ControllerBase
    {
        private readonly IExpenseTypeService _expenseTypeService;

        public ExpenseTypeController(IExpenseTypeService ExpenseTypeService)
        {
            _expenseTypeService = ExpenseTypeService;
        }

        [HttpGet("GetList/{userId}")]
        public async Task<IEnumerable<ExpenseTypeList>> GetList(string userId)
        {
            return await _expenseTypeService.GetExpenseTypes(userId);
        }

        [HttpGet("Get/{id}")]
        public async Task<ExpenseTypeDetails> Get(int id)
        {
            return await _expenseTypeService.GetById(id);
        }
        
        [HttpPost("Create/{userId}")]
        public async Task<bool> Post(string userId, [FromBody]ExpenseTypeDetails createdObj)
        {
            return await _expenseTypeService.CreateExpenseType(createdObj, userId);
        }
        
        [HttpPut("Edit/{id}/{userId}")]
        public async Task<bool> Put(int id, string userId, [FromBody]ExpenseTypeDetails editedObj)
        {
            editedObj.Id = id;
            return await _expenseTypeService.EditExpenseType(editedObj, userId);
        }
        
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _expenseTypeService.DeleteExpenseType(id);
        }
    }
}
