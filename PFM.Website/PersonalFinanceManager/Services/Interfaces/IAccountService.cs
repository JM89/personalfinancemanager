using PersonalFinanceManager.Models.AspNetUserAccount;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IAccountService
    {
        AuthenticatedUser Login(LoginViewModel user);

        string Register(RegisterViewModel user);
    }
}
