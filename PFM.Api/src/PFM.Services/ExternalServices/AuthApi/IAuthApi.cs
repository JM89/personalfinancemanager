using PFM.Authentication.Api.DTOs;
using Refit;
using System.Threading.Tasks;

namespace PFM.Services.ExternalServices.AuthApi
{
    public interface IAuthApi
    {
        [Post("/users/register")]
        Task<UserResponse> Register(UserRequest model);

        [Post("/users/authenticate")]
        Task<UserResponse> Login(UserRequest model);
    }
}
