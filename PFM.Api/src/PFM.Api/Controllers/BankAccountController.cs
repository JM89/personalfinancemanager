﻿using Microsoft.AspNetCore.Mvc;
using PFM.Bank.Api.Contracts.Account;
using PFM.Bank.Api.Contracts.Shared;
using PFM.Services.ExternalServices.BankApi;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankAccountController(IBankAccountApi api) : ControllerBase
    {
        [HttpGet("GetList/{userId}")]
        public async Task<ApiResponse> GetList(string userId)
        {
            return await api.GetList(userId);
        }

        [HttpGet("Get/{id}")]
        public async Task<ApiResponse> Get(int id)
        {
            return await api.Get(id);
        }
        
        [HttpPost("Create/{userId}")]
        public async Task<ApiResponse> Post(string userId, [FromBody]AccountDetails request)
        {
            return await api.Create(userId, request);
        }
        
        [HttpPut("Edit/{id}/{userId}")]
        public async Task<ApiResponse> Put(int id, string userId, [FromBody]AccountDetails request)
        {
            return await api.Edit(id, userId, request);
        }
        
        [HttpDelete("Delete/{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            return await api.Delete(id);
        }

        [HttpPost("SetAsFavorite/{id}")]
        public async Task<ApiResponse> SetAsFavorite(int id)
        {
            return await api.SetAsFavorite(id);
        }
    }
}
