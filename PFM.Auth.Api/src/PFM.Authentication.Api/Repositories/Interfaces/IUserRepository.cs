using PFM.Authentication.Api.Entities;

namespace PFM.Authentication.Api.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        User GetUserByName(string username);
    }
}
