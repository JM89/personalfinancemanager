using PersonalFinanceManager.Services.Core;
using PersonalFinanceManager.Models.UserProfile;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IUserProfileService : IBaseService
    {
        void CreateUserProfile(UserProfileEditModel userProfileEditModel);

        UserProfileEditModel GetById(int id);

        void EditUserProfile(UserProfileEditModel userProfileEditModel);

        UserProfileEditModel GetByUserId(string userId);
    }
}