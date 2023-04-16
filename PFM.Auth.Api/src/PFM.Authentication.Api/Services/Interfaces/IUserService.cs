using PFM.Authentication.Api.DTOs;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PFM.Authentication.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> AuthenticateAsync(string username, string password);
        Task<bool> CreateAsync(UserRequest user);
        Task<UserResponse> GetAuthenticatedUser(ClaimsIdentity identity);
    }
}
