﻿using PersonalFinanceManager.Models.AspNetUserAccount;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using PFM.Authentication.Api.DTOs;

namespace PersonalFinanceManager.Services
{
    public class AccountService : IAccountService
    {
        private readonly Serilog.ILogger _logger;
        private readonly IHttpClientExtended _httpClientExtended;

        public AccountService(Serilog.ILogger logger, IHttpClientExtended httpClientExtended)
        {
            _logger = logger;
            _httpClientExtended = httpClientExtended;
        }

        public UserResponse Login(LoginViewModel user)
        {
            var dto = new UserRequest() { Username = user.Email, Password = user.Password };
            return _httpClientExtended.Post<UserRequest, UserResponse>($"/Account/Login", dto, new HttpClientRequestOptions()
            {
                AuthenticationTokenRequired = false
            });
        }

        public string Register(RegisterViewModel user)
        {
            var dto = new UserRequest() { Username = user.Email, Password = user.Password, FirstName = "", LastName = "" };
            var result = _httpClientExtended.Post<UserRequest, UserResponse>($"/Account/Register", dto, new HttpClientRequestOptions()
            {
                AuthenticationTokenRequired = false
            });
            return result.Token;
        }
    }
}