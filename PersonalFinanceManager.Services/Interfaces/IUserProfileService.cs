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