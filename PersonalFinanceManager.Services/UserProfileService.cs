using PersonalFinanceManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Models.Country;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using PersonalFinanceManager.DataAccess;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.Models.UserProfile;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;

namespace PersonalFinanceManager.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public UserProfileService(IUserProfileRepository userProfileRepository)
        {
            this._userProfileRepository = userProfileRepository;
        }
        
        public void CreateUserProfile(UserProfileEditModel userProfileEditModel)
        {
            var userProfile = Mapper.Map<UserProfileModel>(userProfileEditModel);
            _userProfileRepository.Create(userProfile);
        }

        public UserProfileEditModel GetByUserId(string userId)
        {
            var userProfile = _userProfileRepository.GetList().SingleOrDefault(x => x.User_Id == userId);
            if (userProfile != null)
            {
                return Mapper.Map<UserProfileEditModel>(userProfile);
            }
            return new UserProfileEditModel();
        }

        public void EditUserProfile(UserProfileEditModel userProfileEditModel)
        {
            var userProfile = _userProfileRepository.GetList().AsNoTracking().SingleOrDefault(x => x.Id == userProfileEditModel.Id);
            userProfile = Mapper.Map<UserProfileModel>(userProfileEditModel);
            _userProfileRepository.Update(userProfile);
        }

        public UserProfileEditModel GetById(int id)
        {
            var userProfile = _userProfileRepository.GetById(id);
            return Mapper.Map<UserProfileEditModel>(userProfile);
        }
    }
}