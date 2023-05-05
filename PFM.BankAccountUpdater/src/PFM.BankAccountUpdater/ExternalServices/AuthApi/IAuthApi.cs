using PFM.Authentication.Api.DTOs;
using Refit;

namespace PFM.BankAccountUpdater.ExternalServices.AuthApi
{
    public interface IAuthApi
    {
        [Post("/users/authenticate")]
        Task<UserResponse> Login(UserRequest model);
    }
}
