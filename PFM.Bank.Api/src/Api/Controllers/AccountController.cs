using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PFM.Authentication.Api.DTOs;
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
        public async Task<object> Login([FromBody]UserRequest model)
        {
            using (LogContext.PushProperty("UserName", model.Username))
            {
                var result = await _authApi.Login(model);

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
        public async Task<object> Register([FromBody] UserRequest model)
        {
            using (LogContext.PushProperty("UserName", model.Username))
            {
                var request = new UserRequest() { Username = model.Username, Password = model.Password, FirstName = "", LastName = "" };
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
