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
        ApplicationDbContext db;

        public UserProfileService()
        {
            db = new ApplicationDbContext();
        }
        
        public void CreateUserProfile(UserProfileEditModel userProfileEditModel)
        {
            throw new NotImplementedException();
        }

        public UserProfileEditModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void EditUserProfile(UserProfileEditModel userProfileEditModel)
        {
            throw new NotImplementedException();
        }
    }
}