using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using PFM.Api.Configuration;
using PFM.Api.Contracts.UserAccount;
using Microsoft.Extensions.Logging;
using SerilogTimings;
using Serilog.Context;
using System;
using Microsoft.AspNetCore.Http;

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

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration, Serilog.ILogger logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<object> Login([FromBody]User model)
        {
            try
            {
                ObjectResult response = null;

                using (var op = Operation.Begin("User login"))
                using (LogContext.PushProperty("UserName", model.Email))
                {
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                    if (result.Succeeded)
                    {
                        var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                        var token = TokenFactory.GenerateJwtToken(model.Email, appUser, _configuration);

                        response = Ok(new AuthenticatedUser()
                        {
                            Token = token,
                            UserId = appUser.Id
                        });
                    }
                    else
                    {
                        _logger.Warning("User authentication failed");
                        response = BadRequest("Authentication Failed.");
                    }

                    op.Complete();

                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unhandled Exception");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<object> Register([FromBody]User model)
        {
            try
            {
                ObjectResult response = null;

                using (var op = Operation.Begin("User registration"))
                using (LogContext.PushProperty("UserName", model.Email))
                {
                    var user = new IdentityUser
                    {
                        UserName = model.Email,
                        Email = model.Email
                    };
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, false);
                        response = Ok(TokenFactory.GenerateJwtToken(model.Email, user, _configuration));
                    }
                    else
                    {
                        _logger.Warning("User registration failed");
                        response = BadRequest(result.Errors);
                    }

                    op.Complete();
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unhandled Exception");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
