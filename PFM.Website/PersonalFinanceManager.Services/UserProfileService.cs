using System;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.Models.UserProfile;

namespace PersonalFinanceManager.Services
{
    public class UserProfileService : IUserProfileService
    {
        public void CreateUserProfile(UserProfileEditModel userProfileEditModel)
        {
            throw new NotImplementedException();
        }

        public UserProfileEditModel GetByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public void EditUserProfile(UserProfileEditModel userProfileEditModel)
        {
            throw new NotImplementedException();
        }

        public UserProfileEditModel GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}