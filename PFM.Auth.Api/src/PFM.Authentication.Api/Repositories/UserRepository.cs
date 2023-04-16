using PFM.Authentication.Api.Entities;
using PFM.Authentication.Api.Repositories.Interfaces;
using System;
using System.Linq;

namespace PFM.Authentication.Api.Repositories.Implementations
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly Serilog.ILogger _logger;

        public UserRepository(PFMContext db, Serilog.ILogger logger) : base(db)
        {
            _logger = logger;
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
                _logger.Error(ex, $"Unhandled Exception: {ex.Message}");
                throw ;
            }            
        }
    }
}
