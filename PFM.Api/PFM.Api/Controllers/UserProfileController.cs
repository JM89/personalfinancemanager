using Microsoft.AspNetCore.Mvc;
using PFM.Services.DTOs.UserProfile;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/UserProfile")]
    public class UserProfileController : Controller
    {
        private readonly IUserProfileService _UserProfileService;

        public UserProfileController(IUserProfileService UserProfileService)
        {
            _UserProfileService = UserProfileService;
        }

        [HttpGet("Get/{id}")]
        public UserProfileDetails Get(int id)
        {
            return _UserProfileService.GetById(id);
        }

        [HttpGet("GetByUserId/{userId}")]
        public UserProfileDetails GetByUserId(string userId)
        {
            return _UserProfileService.GetByUserId(userId);
        }

        [HttpPost("Create")]
        public void Post([FromBody]UserProfileDetails createdObj)
        {
            _UserProfileService.CreateUserProfile(createdObj);
        }
        
        [HttpPut("Edit/{id}")]
        public void Put(int id, [FromBody]UserProfileDetails editedObj)
        {
            _UserProfileService.EditUserProfile(editedObj);
        }
    }
}
