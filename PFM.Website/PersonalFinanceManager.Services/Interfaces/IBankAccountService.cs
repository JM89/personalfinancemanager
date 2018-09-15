using System.Collections.Generic;
using PersonalFinanceManager.Models.Account;
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IBankAccountService : IBaseService
    {
        void CreateBankAccount(AccountEditModel accountEditModel, string userId);

        IList<AccountListModel> GetAccountsByUser(string userId);

        AccountEditModel GetById(int id);

        void EditBankAccount(AccountEditModel accountEditModel, string userId);

        void DeleteBankAccount(int id);

        void SetAsFavorite(int id);
    }
}