using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PFM.Authentication.Api.DTOs;
using PFM.Authentication.Api.Entities;
using PFM.Authentication.Api.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PFM.Authentication.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]UserRequest model)
        {
            var user = await _userService.AuthenticateAsync(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]UserRequest model)
        {
            var result = await _userService.CreateAsync(model);

            if (result)
            {
                return await Authenticate(model);
            }

            return null;
        }

        [HttpGet]
        [HttpGet("GetUserInfo")]
        public async Task<IActionResult> GetUserInfoAsync()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
            {
                return Unauthorized();
            }
            
            var apiUser = await _userService.GetAuthenticatedUser(identity);

            return Ok(apiUser);
        }
    }
}
