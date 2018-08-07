using System.Linq;
using PFM.DataAccessLayer.Entities;
using AutoMapper;
using PFM.Services.Interfaces;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DTOs.UserProfile;

namespace PFM.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public UserProfileService(IUserProfileRepository userProfileRepository)
        {
            this._userProfileRepository = userProfileRepository;
        }
        
        public void CreateUserProfile(UserProfileDetails userProfileDetails)
        {
            var userProfile = Mapper.Map<UserProfile>(userProfileDetails);
            _userProfileRepository.Create(userProfile);
        }

        public UserProfileDetails GetByUserId(string userId)
        {
            var userProfile = _userProfileRepository.GetList().SingleOrDefault(x => x.User_Id == userId);
            if (userProfile != null)
            {
                return Mapper.Map<UserProfileDetails>(userProfile);
            }
            return new UserProfileDetails();
        }

        public void EditUserProfile(UserProfileDetails userProfileDetails)
        {
            var userProfile = _userProfileRepository.GetListAsNoTracking().SingleOrDefault(x => x.Id == userProfileDetails.Id);
            userProfile = Mapper.Map<UserProfile>(userProfileDetails);
            _userProfileRepository.Update(userProfile);
        }

        public UserProfileDetails GetById(int id)
        {
            var userProfile = _userProfileRepository.GetById(id);
            return Mapper.Map<UserProfileDetails>(userProfile);
        }
    }
}