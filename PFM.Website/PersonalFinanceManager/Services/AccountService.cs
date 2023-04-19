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
                var dto = new UserRequest() { Username = user.Email, Password = user.Password };
                result = httpClient.Post<UserRequest, UserResponse>($"/Account/Login", dto, new HttpClientRequestOptions() {
                    AuthenticationTokenRequired = false
                });
            }
            return result;
        }

        public string Register(RegisterViewModel user)
        {
            UserResponse result = null;
            using (var httpClient = new HttpClientExtended(_logger))
            {
                var dto = new UserRequest() { Username = user.Email, Password = user.Password, FirstName = "", LastName = "" };
                result = httpClient.Post<UserRequest, UserResponse>($"/Account/Register", dto, new HttpClientRequestOptions()
                {
                    AuthenticationTokenRequired = false
                });
            }
            return result.Token;
        }
    }
}