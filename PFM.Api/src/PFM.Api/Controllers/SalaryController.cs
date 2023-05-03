using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.Salary;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalaryController : ControllerBase
    {
        private readonly ISalaryService _salaryService;

        public SalaryController(ISalaryService SalaryService)
        {
            _salaryService = SalaryService;
        }

        [HttpGet("GetList/{userId}")]
        public async Task<IList<SalaryList>> GetList(string userId)
        {
            return await _salaryService.GetSalaries(userId);
        }

        [HttpGet("Get/{id}")]
        public Task<SalaryDetails> Get(int id)
        {
            return _salaryService.GetById(id);
        }
        
        [HttpPost("Create")]
        public Task<bool> Post([FromBody]SalaryDetails createdObj)
        {
            return _salaryService.CreateSalary(createdObj);
        }
        
        [HttpPut("Edit/{id}")]
        public Task<bool> Put(int id, [FromBody]SalaryDetails editedObj)
        {
            return _salaryService.EditSalary(editedObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public Task<bool> Delete(int id)
        {
            return _salaryService.DeleteSalary(id);
        }

        [HttpPost("CopySalary/{sourceId}")]
        public Task<bool> CopySalary(int sourceId)
        {
            return _salaryService.CopySalary(sourceId);
        }
    }
}
