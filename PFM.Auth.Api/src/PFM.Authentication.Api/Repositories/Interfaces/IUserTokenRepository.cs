namespace PFM.Authentication.Api.Repositories.Interfaces
{
    public interface IUserTokenRepository
    {
        bool ValidateToken(string username, string token);
    }
}