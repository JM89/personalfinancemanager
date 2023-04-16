using PFM.Authentication.Api.Entities;
using PFM.Authentication.Api.Repositories.Interfaces;
using System;
using System.Linq;

namespace PFM.Authentication.Api.Repositories.Implementations
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(PFMContext db) : base(db)
        {

        }

        public User GetUserByName(string username)
        {
            try
            {
                var users = GetList().ToList();

                return users.SingleOrDefault(x => x.Username == username);
            }
            catch(Exception ex)
            {
                // TODO: add logging
                throw ;
            }            
        }
    }
}
