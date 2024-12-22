using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;

namespace PFM.DataAccessLayer.Repositories.Implementations
{
    public class UserProfileRepository(PFMContext db) : BaseRepository<UserProfile>(db), IUserProfileRepository;
}
