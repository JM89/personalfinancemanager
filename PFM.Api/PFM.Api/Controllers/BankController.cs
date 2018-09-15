using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PFM.DTOs.Bank;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Bank")]
    public class BankController : Controller
    {
        private readonly IBankService _BankService;

        public BankController(IBankService BankService)
        {
            _BankService = BankService;
        }

        [HttpGet("GetList")]
        public IEnumerable<BankList> GetList()
        {
            return _BankService.GetBanks();
        }

        [HttpGet("Get/{id}")]
        public BankDetails Get(int id)
        {
            return _BankService.GetById(id);
        }
        
        [HttpPost("Create")]
        public void Post([FromBody]BankDetails createdObj)
        {
            _BankService.CreateBank(createdObj);
        }
        
        [HttpPut("Edit/{id}")]
        public void Put(int id, [FromBody]BankDetails editedObj)
        {
            _BankService.EditBank(editedObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public void Delete(int id)
        {
            _BankService.DeleteBank(id);
        }
    }
}
