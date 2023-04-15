using PersonalFinanceManager.Models.AspNetUserAccount;
using PersonalFinanceManager.Services.HttpClientWrapper;
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Services
{
    public class AccountService : IAccountService
    {
        public AuthenticatedUser Login(LoginViewModel user)
        {
            AuthenticatedUser result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.UserAccount.User>(user);
                result = httpClient.Post<PFM.Api.Contracts.UserAccount.User, AuthenticatedUser>($"/Account/Login", dto, new HttpClientRequestOptions() {
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