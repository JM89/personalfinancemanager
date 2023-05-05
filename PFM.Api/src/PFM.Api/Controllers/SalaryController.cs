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
        public async Task<SalaryDetails> Get(int id)
        {
            return await _salaryService.GetById(id);
        }
        
        [HttpPost("Create")]
        public async Task<bool> Post([FromBody]SalaryDetails createdObj)
        {
            return await _salaryService.CreateSalary(createdObj);
        }
        
        [HttpPut("Edit/{id}")]
        public async Task<bool> Put(int id, [FromBody]SalaryDetails editedObj)
        {
            editedObj.Id = id;
            return await _salaryService.EditSalary(editedObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _salaryService.DeleteSalary(id);
        }

        [HttpPost("CopySalary/{sourceId}")]
        public async Task<bool> CopySalary(int sourceId)
        {
            return await _salaryService.CopySalary(sourceId);
        }
    }
}
