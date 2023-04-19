using System;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.Models.UserProfile;
using PersonalFinanceManager.Services.HttpClientWrapper;

namespace PersonalFinanceManager.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly Serilog.ILogger _logger;
        private readonly IHttpClientExtended _httpClientExtended;

        public UserProfileService(Serilog.ILogger logger, IHttpClientExtended httpClientExtended)
        {
            _logger = logger;
            _httpClientExtended = httpClientExtended;
        }

        public void CreateUserProfile(UserProfileEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.UserProfile.UserProfileDetails>(model);
            _httpClientExtended.Post($"/UserProfile/Create", dto);
        }

        public UserProfileEditModel GetByUserId(string userId)
        {
            UserProfileEditModel result = null;
            var response = _httpClientExtended.GetSingle<PFM.Api.Contracts.UserProfile.UserProfileDetails>($"/UserProfile/GetByUserId/{userId}");
            result = AutoMapper.Mapper.Map<UserProfileEditModel>(response);
            return result;
        }

        public void EditUserProfile(UserProfileEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.UserProfile.UserProfileDetails>(model);
            _httpClientExtended.Put($"/UserProfile/Edit/{model.Id}", dto);
        }

        public UserProfileEditModel GetById(int id)
        {
            UserProfileEditModel result = null;
            var response = _httpClientExtended.GetSingle<PFM.Api.Contracts.UserProfile.UserProfileDetails>($"/UserProfile/Get/{id}");
            result = AutoMapper.Mapper.Map<UserProfileEditModel>(response);
            return result;
        }
    }
}