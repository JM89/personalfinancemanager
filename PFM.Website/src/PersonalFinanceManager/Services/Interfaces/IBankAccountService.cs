using PersonalFinanceManager.Models.Account;
using PersonalFinanceManager.Services.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IBankAccountService : IBaseService
    {
        Task<bool> CreateBankAccount(AccountEditModel accountEditModel, string userId);

        Task<IList<AccountListModel>> GetAccountsByUser(string userId);

        Task<AccountEditModel> GetById(int id);

        Task<bool> EditBankAccount(AccountEditModel accountEditModel, string userId);

        Task<bool> DeleteBankAccount(int id);

        Task<bool> SetAsFavorite(int id);
    }
}