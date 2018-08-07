using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PFM.DTOs.Saving;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Saving")]
    public class SavingController : Controller
    {
        private readonly ISavingService _SavingService;

        public SavingController(ISavingService SavingService)
        {
            _SavingService = SavingService;
        }

        [HttpGet("GetList/{accountId}")]
        public IEnumerable<SavingList> GetList(int accountId)
        {
            return _SavingService.GetSavingsByAccountId(accountId);
        }

        [HttpGet("Get/{id}")]
        public SavingDetails Get(int id)
        {
            return _SavingService.GetById(id);
        }
        
        [HttpPost("Create")]
        public void Post([FromBody]SavingDetails createdObj)
        {
            _SavingService.CreateSaving(createdObj);
        }
        
        [HttpPut("Edit/{id}")]
        public void Put(int id, [FromBody]SavingDetails editedObj)
        {
            _SavingService.EditSaving(editedObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public void Delete(int id)
        {
            _SavingService.DeleteSaving(id);
        }
    }
}
