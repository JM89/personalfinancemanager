﻿using Microsoft.AspNetCore.Mvc;
using PFM.Api.Contracts.UserProfile;
using PFM.Services.Interfaces;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
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
