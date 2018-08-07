using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PFM.DTOs.Salary;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Salary")]
    public class SalaryController : Controller
    {
        private readonly ISalaryService _SalaryService;

        public SalaryController(ISalaryService SalaryService)
        {
            _SalaryService = SalaryService;
        }

        [HttpGet("GetList/{userId}")]
        public IEnumerable<SalaryList> GetList(string userId)
        {
            return _SalaryService.GetSalaries(userId);
        }

        [HttpGet("Get/{id}")]
        public SalaryDetails Get(int id)
        {
            return _SalaryService.GetById(id);
        }
        
        [HttpPost("Create")]
        public void Post([FromBody]SalaryDetails createdObj)
        {
            _SalaryService.CreateSalary(createdObj);
        }
        
        [HttpPut("Edit/{id}")]
        public void Put(int id, [FromBody]SalaryDetails editedObj)
        {
            _SalaryService.EditSalary(editedObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public void Delete(int id)
        {
            _SalaryService.DeleteSalary(id);
        }
    }
}
