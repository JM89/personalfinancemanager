using System;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.Models.UserProfile;
using PersonalFinanceManager.Services.HttpClientWrapper;
using System.Threading.Tasks;

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

        public async Task<bool> CreateUserProfile(UserProfileEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.UserProfile.UserProfileDetails>(model);
            return await _httpClientExtended.Post($"/UserProfile/Create", dto);
        }

        public async Task<UserProfileEditModel> GetByUserId(string userId)
        {
            var response = await _httpClientExtended.GetSingle<PFM.Api.Contracts.UserProfile.UserProfileDetails>($"/UserProfile/GetByUserId/{userId}");
            return AutoMapper.Mapper.Map<UserProfileEditModel>(response);
        }

        public async Task<bool> EditUserProfile(UserProfileEditModel model)
        {
            var dto = AutoMapper.Mapper.Map<PFM.Api.Contracts.UserProfile.UserProfileDetails>(model);
            return await _httpClientExtended.Put($"/UserProfile/Edit/{model.Id}", dto);
        }

        public async Task<UserProfileEditModel> GetById(int id)
        {
            var response = await _httpClientExtended.GetSingle<PFM.Api.Contracts.UserProfile.UserProfileDetails>($"/UserProfile/Get/{id}");
            return AutoMapper.Mapper.Map<UserProfileEditModel>(response);
        }
    }
}