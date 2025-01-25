using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.ExpenseType;
using PFM.Services;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseTypeController(IExpenseTypeService service) : ControllerBase
    {
        [HttpGet("GetList/{userId}")]
        public async Task<IEnumerable<ExpenseTypeList>> GetList(string userId)
        {
            return await service.GetExpenseTypes(userId);
        }

        [HttpGet("Get/{id}")]
        public async Task<ExpenseTypeDetails> Get(int id)
        {
            return await service.GetById(id);
        }
        
        [HttpPost("Create/{userId}")]
        public async Task<bool> Post(string userId, [FromBody]ExpenseTypeDetails createdObj)
        {
            return await service.CreateExpenseType(createdObj, userId);
        }
        
        [HttpPut("Edit/{id}/{userId}")]
        public async Task<bool> Put(int id, string userId, [FromBody]ExpenseTypeDetails editedObj)
        {
            editedObj.Id = id;
            return await service.EditExpenseType(editedObj, userId);
        }
        
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await service.DeleteExpenseType(id);
        }
    }
}
