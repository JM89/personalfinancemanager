using PFM.Authentication.Api.DTOs;
using PFM.Authentication.Api.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PFM.Authentication.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> AuthenticateAsync(string username, string password);
        Task<User> CreateAsync(UserRequest user);
        Task<UserResponse> GetAuthenticatedUser(ClaimsIdentity identity);
    }
}
