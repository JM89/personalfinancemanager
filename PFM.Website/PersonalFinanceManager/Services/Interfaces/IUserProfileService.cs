using PersonalFinanceManager.Services.Core;
using PersonalFinanceManager.Models.UserProfile;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IUserProfileService : IBaseService
    {
        Task<bool> CreateUserProfile(UserProfileEditModel userProfileEditModel);

        Task<UserProfileEditModel> GetById(int id);

        Task<bool> EditUserProfile(UserProfileEditModel userProfileEditModel);

        Task<UserProfileEditModel> GetByUserId(string userId);
    }
}