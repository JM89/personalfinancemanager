﻿using PFM.Api.Contracts.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PFM.Services.Interfaces
{
    public interface IBankAccountService : IBaseService
    {
        Task<bool> CreateBankAccount(AccountDetails accountDetails, string userId);

        IList<AccountList> GetAccountsByUser(string userId);

        AccountDetails GetById(int id);

        Task<bool> EditBankAccount(AccountDetails accountDetails, string userId);

        Task<bool> DeleteBankAccount(int id);

        void SetAsFavorite(int id);
    }
}