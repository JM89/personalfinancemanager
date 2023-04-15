using System;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.Models.UserProfile;
using PersonalFinanceManager.Services.HttpClientWrapper;

namespace PersonalFinanceManager.Services
{
    public class UserProfileService : IUserProfileService
    {
        public void CreateUserProfile(UserProfileEditModel model)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.UserProfile.UserProfileDetails>(model);
                httpClient.Post($"/UserProfile/Create", dto);
            }
        }

        public UserProfileEditModel GetByUserId(string userId)
        {
            UserProfileEditModel result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetSingle<PFM.Api.Contracts.UserProfile.UserProfileDetails>($"/UserProfile/GetByUserId/{userId}");
                result = AutoMapper.Mapper.Map<UserProfileEditModel>(response);
            }
            return result;
        }

        public void EditUserProfile(UserProfileEditModel model)
        {
            using (var httpClient = new HttpClientExtended())
            {
                var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.UserProfile.UserProfileDetails>(model);
                httpClient.Put($"/UserProfile/Edit/{model.Id}", dto);
            }
        }

        public UserProfileEditModel GetById(int id)
        {
            UserProfileEditModel result = null;
            using (var httpClient = new HttpClientExtended())
            {
                var response = httpClient.GetSingle<PFM.Api.Contracts.UserProfile.UserProfileDetails>($"/UserProfile/Get/{id}");
                result = AutoMapper.Mapper.Map<UserProfileEditModel>(response);
            }
            return result;
        }
    }
}