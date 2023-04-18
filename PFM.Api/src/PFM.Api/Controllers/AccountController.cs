﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PFM.Api.Configuration;
using PFM.Api.Contracts.UserAccount;
using PFM.Services.ExternalServices.AuthApi;
using Serilog.Context;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PFM.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly Serilog.ILogger _logger;
        private readonly IAuthApi _authApi;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration, Serilog.ILogger logger, IAuthApi authApi)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
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
