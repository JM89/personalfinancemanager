using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PFM.Api.Configuration;
using PFM.Api.Contracts.UserAccount;
using Serilog.Context;
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
            using (LogContext.PushProperty("UserName", model.Email))
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if (result.Succeeded)
                {
                    var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                    var token = TokenFactory.GenerateJwtToken(model.Email, appUser, _configuration);

                    return new AuthenticatedUser()
                    {
                        Token = token,
                        UserId = appUser.Id
                    };
                }
                _logger.Warning("User authentication failed");
                return BadRequest("Authentication Failed.");
            }
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<object> Register([FromBody]User model)
        {
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
                    return TokenFactory.GenerateJwtToken(model.Email, user, _configuration);
                }

                _logger.Warning("User registration failed");
                return BadRequest(result.Errors);
            }
        }
    }
}
