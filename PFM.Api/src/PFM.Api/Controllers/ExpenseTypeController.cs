using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.ExpenseType;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseTypeController : ControllerBase
    {
        private readonly IExpenseTypeService _ExpenseTypeService;

        public ExpenseTypeController(IExpenseTypeService ExpenseTypeService)
        {
            _ExpenseTypeService = ExpenseTypeService;
        }

        [HttpGet("GetList")]
        public async Task<IEnumerable<ExpenseTypeList>> GetList()
        {
            return await _ExpenseTypeService.GetExpenseTypes();
        }

        [HttpGet("Get/{id}")]
        public async Task<ExpenseTypeDetails> Get(int id)
        {
            return await _ExpenseTypeService.GetById(id);
        }
        
        [HttpPost("Create")]
        public async Task<bool> Post([FromBody]ExpenseTypeDetails createdObj)
        {
            return await _ExpenseTypeService.CreateExpenseType(createdObj);
        }
        
        [HttpPut("Edit/{id}")]
        public async Task<bool> Put(int id, [FromBody]ExpenseTypeDetails editedObj)
        {
            return await _ExpenseTypeService.EditExpenseType(editedObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _ExpenseTypeService.DeleteExpenseType(id);
        }
    }
}
