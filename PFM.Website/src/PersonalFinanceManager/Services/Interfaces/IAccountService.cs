using PersonalFinanceManager.Models.AspNetUserAccount;
using PFM.Authentication.Api.DTOs;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IAccountService
    {
        Task<UserResponse> Login(LoginViewModel user);

        Task<string> Register(RegisterViewModel user);
    }
}
