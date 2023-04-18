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
                var row_count = _db.Set<UserToken>().Count(x => x.Username == userToken.Username && x.Token == userToken.Token);

                return row_count == 1;
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
                var result = _db.Set<UserToken>().Count(x => x.Username == x.Username) == 0 ? Create(userToken) : Update(userToken);

                return result != null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Unhandled Exception: {ex.Message}");
                throw;
            }
        }
    }
}
