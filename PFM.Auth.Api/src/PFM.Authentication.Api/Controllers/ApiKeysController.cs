using Microsoft.AspNetCore.Mvc;
using PFM.Authentication.Api.Helpers;
using PFM.Authentication.Api.Models;
using PFM.Authentication.Api.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace PFM.Authentication.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiKeysController : ControllerBase
    {
        private readonly ISecretManagerService _secretManagerService;

        public ApiKeysController(ISecretManagerService secretManagerService)
        {
            _secretManagerService = secretManagerService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAppAsync([FromBody]RegisterAppRequest apiKeyInfo)
        {
            if (apiKeyInfo.AppId == Guid.Empty)
            {
                return BadRequest("Invalid APP Id");
            }

            var newApiKey = RandomApiKeys.RandomString(50);
            var result = await _secretManagerService.CreateApiKeySecrets(apiKeyInfo.AppId, apiKeyInfo.AppName, newApiKey);

            if (result)
            {
                return Ok(newApiKey);
            }

            return Conflict("API Already Registered");
        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateApiKey([FromBody]ValidateApiKeyRequest apiKeyInfo)
        {
            var result = await _secretManagerService.GetApiKeySecrets(apiKeyInfo.AppId);
            if (result == null)
            {
                return BadRequest("Invalid App ID");
            }
            return Ok(result == apiKeyInfo.ApiKey);
        }
    }
}
