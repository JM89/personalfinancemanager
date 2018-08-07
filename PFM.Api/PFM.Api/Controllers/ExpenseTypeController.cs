using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PFM.DTOs.ExpenseType;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/ExpenseType")]
    public class ExpenseTypeController : Controller
    {
        private readonly IExpenseTypeService _ExpenseTypeService;

        public ExpenseTypeController(IExpenseTypeService ExpenseTypeService)
        {
            _ExpenseTypeService = ExpenseTypeService;
        }

        [HttpGet("GetList")]
        public IEnumerable<ExpenseTypeList> GetList()
        {
            return _ExpenseTypeService.GetExpenseTypes();
        }

        [HttpGet("Get/{id}")]
        public ExpenseTypeDetails Get(int id)
        {
            return _ExpenseTypeService.GetById(id);
        }
        
        [HttpPost("Create")]
        public void Post([FromBody]ExpenseTypeDetails createdObj)
        {
            _ExpenseTypeService.CreateExpenseType(createdObj);
        }
        
        [HttpPut("Edit/{id}")]
        public void Put(int id, [FromBody]ExpenseTypeDetails editedObj)
        {
            _ExpenseTypeService.EditExpenseType(editedObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public void Delete(int id)
        {
            _ExpenseTypeService.DeleteExpenseType(id);
        }
    }
}
