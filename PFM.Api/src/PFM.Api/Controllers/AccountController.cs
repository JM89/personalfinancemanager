using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.UserAccount;
using PFM.Services.ExternalServices.AuthApi;
using Serilog.Context;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly IAuthApi _authApi;

        public AccountController(Serilog.ILogger logger, IAuthApi authApi)
        {
            _logger = logger;
            _authApi = authApi;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<object> Login([FromBody]User model)
        {
            using (LogContext.PushProperty("UserName", model.Email))
            {
                var request = new Authentication.Api.DTOs.UserRequest() { Username = model.Email, Password = model.Password };
                var result = await _authApi.Login(request);

                if (result != null)
                {
                    return Ok(result);
                }

                _logger.Warning("User authentication failed");
                return BadRequest("User authentication failed");
            }
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<object> Register([FromBody]User model)
        {
            using (LogContext.PushProperty("UserName", model.Email))
            {
                var request = new Authentication.Api.DTOs.UserRequest() { Username = model.Email, Password = model.Password, FirstName = "", LastName = "" };
                var result = await _authApi.Register(request);

                if (result != null)
                {
                    return Ok(result);
                }

                _logger.Warning("User registration failed");
                return BadRequest("User registration failed");
            }
        }
    }
}
