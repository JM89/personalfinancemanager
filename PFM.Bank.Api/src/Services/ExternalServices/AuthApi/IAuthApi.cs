﻿using PFM.Authentication.Api.DTOs;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PFM.Services.ExternalServices.AuthApi
{
    public interface IAuthApi
    {
        [Post("/users/register")]
        Task<UserResponse> Register(UserRequest model);

        [Post("/users/authenticate")]
        Task<UserResponse> Login(UserRequest model);

        [Get("/users/validatetoken")]
        Task<bool> ValidateToken([Header("Authorization")] string bearerToken);
    }
}