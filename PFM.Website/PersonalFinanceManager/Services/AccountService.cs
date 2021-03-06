﻿using System.Collections.Generic;
using PersonalFinanceManager.Models.AtmWithdraw;
using PersonalFinanceManager.Services.Interfaces;
using System;
using PersonalFinanceManager.Services.HttpClientWrapper;
using System.Linq;
using PersonalFinanceManager.Models.AspNetUserAccount;

namespace PersonalFinanceManager.Services
{
    public class AccountService : IAccountService
    {
        public AuthenticatedUser Login(LoginViewModel user)
        {
            AuthenticatedUser result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PersonalFinanceManager.DTOs.UserAccount.User>(user);
                result = httpClient.Post<PersonalFinanceManager.DTOs.UserAccount.User, AuthenticatedUser>($"/Account/Login", dto, new HttpClientRequestOptions() {
                    AuthenticationTokenRequired = false
                });
            }
            return result;
        }

        public string Register(RegisterViewModel user)
        {
            string result = "";
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PersonalFinanceManager.DTOs.UserAccount.User>(user);
                result = httpClient.Post<PersonalFinanceManager.DTOs.UserAccount.User, string>($"/Account/Register", dto, new HttpClientRequestOptions()
                {
                    AuthenticationTokenRequired = false
                });
            }
            return result;
        }
    }
}