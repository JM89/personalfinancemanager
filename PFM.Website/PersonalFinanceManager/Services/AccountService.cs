using PersonalFinanceManager.Models.AspNetUserAccount;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using PFM.Authentication.Api.DTOs;

namespace PersonalFinanceManager.Services
{
    public class AccountService : IAccountService
    {
        private readonly Serilog.ILogger _logger;

        public AccountService(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public UserResponse Login(LoginViewModel user)
        {
            UserResponse result = null;
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.UserAccount.User>(user);
                result = httpClient.Post<PFM.Api.Contracts.UserAccount.User, UserResponse>($"/Account/Login", dto, new HttpClientRequestOptions() {
                    AuthenticationTokenRequired = false
                });
            }
            return result;
        }

        public string Register(RegisterViewModel user)
        {
            string result = "";
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.UserAccount.User>(user);
                result = httpClient.Post<PFM.Api.Contracts.UserAccount.User, string>($"/Account/Register", dto, new HttpClientRequestOptions()
                {
                    AuthenticationTokenRequired = false
                });
            }
            return result;
        }
    }
}