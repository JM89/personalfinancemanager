using PersonalFinanceManager.Models.AspNetUserAccount;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;
using PFM.Authentication.Api.DTOs;
using System.Threading.Tasks;

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

        public async Task<UserResponse> Login(LoginViewModel user)
        {
            var dto = new UserRequest() { Username = user.Email, Password = user.Password };
            return await _httpClientExtended.Post<UserRequest, UserResponse>($"/Account/Login", dto, new HttpClientRequestOptions()
            {
                AuthenticationTokenRequired = false
            });
        }

        public async Task<string> Register(RegisterViewModel user)
        {
            var dto = new UserRequest() { Username = user.Email, Password = user.Password, FirstName = "", LastName = "" };
            var result = await _httpClientExtended.Post<UserRequest, UserResponse>($"/Account/Register", dto, new HttpClientRequestOptions()
            {
                AuthenticationTokenRequired = false
            });
            return result.Token;
        }
    }
}