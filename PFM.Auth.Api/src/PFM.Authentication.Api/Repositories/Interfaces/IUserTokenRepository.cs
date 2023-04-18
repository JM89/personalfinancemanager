using PFM.Authentication.Api.Entities;

namespace PFM.Authentication.Api.Repositories.Interfaces
{
    public interface IUserTokenRepository
    {
        bool ValidateToken(UserToken userToken);

        bool SaveToken(UserToken userToken);
    }
}