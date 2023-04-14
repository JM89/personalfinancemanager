using PFM.Services.DTOs.UserProfile;

namespace PFM.Services.Interfaces
{
    public interface IUserProfileService : IBaseService
    {
        void CreateUserProfile(UserProfileDetails userProfileDetails);

        UserProfileDetails GetById(int id);

        void EditUserProfile(UserProfileDetails userProfileDetails);

        UserProfileDetails GetByUserId(string userId);
    }
}