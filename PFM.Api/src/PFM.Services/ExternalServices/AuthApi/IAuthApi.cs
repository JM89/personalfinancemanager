using PFM.Authentication.Api.DTOs;
using Refit;
using System.Threading.Tasks;

namespace PFM.Services.ExternalServices.AuthApi
{
    public interface IAuthApi
    {
        [Get("/users/register")]
        Task<UserResponse> Register(UserRequest model);

        [Get("/users/login")]
        Task<UserResponse> Login(UserRequest model);
    }
}
