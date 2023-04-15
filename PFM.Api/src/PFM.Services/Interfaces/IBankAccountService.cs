using System.Collections.Generic;
using PFM.Api.Contracts.Account;

namespace PFM.Services.Interfaces
{
    public interface IBankAccountService : IBaseService
    {
        void CreateBankAccount(AccountDetails accountDetails, string userId);

        IList<AccountList> GetAccountsByUser(string userId);

        AccountDetails GetById(int id);

        void EditBankAccount(AccountDetails accountDetails, string userId);

        void DeleteBankAccount(int id);

        void SetAsFavorite(int id);
    }
}