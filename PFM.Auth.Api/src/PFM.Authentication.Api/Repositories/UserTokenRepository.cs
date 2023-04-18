using PFM.Authentication.Api.Entities;
using PFM.Authentication.Api.Repositories.Interfaces;
using System;
using System.Linq;

namespace PFM.Authentication.Api.Repositories.Implementations
{
    public class UserTokenRepository : BaseRepository<UserToken>, IUserTokenRepository
    {
        private readonly Serilog.ILogger _logger;

        public UserTokenRepository(PFMContext db, Serilog.ILogger logger) : base(db)
        {
            _logger = logger;
        }

        public bool ValidateToken(UserToken userToken)
        {
            try
            {
                var userTokens = _db.Set<UserToken>().Where(x => x.Username == userToken.Username && x.Token == userToken.Token);

                return userTokens.Any();
            }
            catch(Exception ex)
            {
                _logger.Error(ex, $"Unhandled Exception: {ex.Message}");
                throw ;
            }            
        }

        public bool SaveToken(UserToken userToken)
        {
            try
            {
                var existingUserToken = _db.Set<UserToken>().SingleOrDefault(x => x.Username == x.Username);

                if (existingUserToken != null)
                {
                    existingUserToken.Token = userToken.Token;
                    return Update(existingUserToken) != null;
                }

                return Create(userToken) != null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Unhandled Exception: {ex.Message}");
                throw;
            }
        }
    }
}
