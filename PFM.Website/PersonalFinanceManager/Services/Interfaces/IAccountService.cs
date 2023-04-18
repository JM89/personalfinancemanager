using PersonalFinanceManager.Models.AspNetUserAccount;
using PFM.Authentication.Api.DTOs;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IAccountService
    {
        UserResponse Login(LoginViewModel user);

        string Register(RegisterViewModel user);
    }
}
