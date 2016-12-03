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

namespace PersonalFinanceManager.Services
{
    public class UserProfileService : IUserProfileService
    {
        private ApplicationDbContext _db;

        public UserProfileService(ApplicationDbContext db)
        {
            this._db = db;
        }
        
        public void CreateUserProfile(UserProfileEditModel userProfileEditModel)
        {
            var userProfile = Mapper.Map<UserProfileModel>(userProfileEditModel);

            _db.UserProfileModels.Add(userProfile);
            _db.SaveChanges();
        }

        public UserProfileEditModel GetByUserId(string userId)
        {
            var userProfile = _db.UserProfileModels.SingleOrDefault(x => x.User_Id == userId);
            if (userProfile != null)
            {
                return Mapper.Map<UserProfileEditModel>(userProfile);
            }
            return new UserProfileEditModel();
        }

        public void EditUserProfile(UserProfileEditModel userProfileEditModel)
        {
            var userProfile = _db.UserProfileModels.AsNoTracking().SingleOrDefault(x => x.Id == userProfileEditModel.Id);
            userProfile = Mapper.Map<UserProfileModel>(userProfileEditModel);

            _db.Entry(userProfile).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public UserProfileEditModel GetById(int id)
        {
            var userProfile = _db.UserProfileModels.Single(x => x.Id == id);
            return Mapper.Map<UserProfileEditModel>(userProfile);
        }
    }
}