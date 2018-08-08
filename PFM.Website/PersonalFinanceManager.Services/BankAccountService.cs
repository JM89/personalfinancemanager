using System;
using System.Collections.Generic;
using PersonalFinanceManager.Models.Account;
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Services
{
    public class BankAccountService: IBankAccountService
    {
        public void CreateBankAccount(AccountEditModel accountEditModel, string userId)
        {
            throw new NotImplementedException();
        }

        public IList<AccountListModel> GetAccountsByUser(string userId)
        {
            throw new NotImplementedException();
        }
        
        public AccountEditModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void EditBankAccount(AccountEditModel accountEditModel, string userId)
        {
            throw new NotImplementedException();
        }

        public void DeleteBankAccount(int id)
        {
            throw new NotImplementedException();
        }
        
        public void SetAsFavorite(int id)
        {
            throw new NotImplementedException();
        }
    }
}