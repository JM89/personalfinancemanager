using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PFM.DTOs.Income;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Income")]
    public class IncomeController : Controller
    {
        private readonly IIncomeService _IncomeService;

        public IncomeController(IIncomeService IncomeService)
        {
            _IncomeService = IncomeService;
        }

        [HttpGet("GetList/{accountId}")]
        public IEnumerable<IncomeList> GetList(int accountId)
        {
            return _IncomeService.GetIncomes(accountId);
        }

        [HttpGet("Get/{id}")]
        public IncomeDetails Get(int id)
        {
            return _IncomeService.GetById(id);
        }
        
        [HttpPost("Create")]
        public void Post([FromBody]IncomeDetails createdObj)
        {
            _IncomeService.CreateIncome(createdObj);
        }
        
        [HttpPut("Edit/{id}")]
        public void Put(int id, [FromBody]IncomeDetails editedObj)
        {
            _IncomeService.EditIncome(editedObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public void Delete(int id)
        {
            _IncomeService.DeleteIncome(id);
        }
        
        [HttpPost("CreateIncomes")]
        public void CreateIncomes([FromBody]List<IncomeDetails> createdObjs)
        {
            _IncomeService.CreateIncomes(createdObjs);
        }
    }
}
