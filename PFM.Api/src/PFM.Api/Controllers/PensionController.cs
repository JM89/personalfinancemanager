﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.Pension;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PensionController : ControllerBase
    {
        private readonly IPensionService _PensionService;

        public PensionController(IPensionService PensionService)
        {
            _PensionService = PensionService;
        }

        [HttpGet("GetList/{userId}")]
        public IEnumerable<PensionList> GetList(string userId)
        {
            return _PensionService.GetPensions(userId);
        }

        [HttpGet("Get/{id}")]
        public PensionDetails Get(int id)
        {
            return _PensionService.GetById(id);
        }
        
        [HttpPost("Create")]
        public void Post([FromBody]PensionDetails createdObj)
        {
            _PensionService.CreatePension(createdObj);
        }
        
        [HttpPut("Edit/{id}")]
        public void Put(int id, [FromBody]PensionDetails editedObj)
        {
            _PensionService.EditPension(editedObj);
        }
        
        [HttpDelete("Delete/{id}")]
        public void Delete(int id)
        {
            _PensionService.DeletePension(id);
        }
    }
}
