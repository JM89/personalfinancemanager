using PersonalFinanceManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Models.Account;
using AutoMapper;
using PersonalFinanceManager.DataAccess;
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IBankAccountService : IBaseService
    {
        void CreateBankAccount(AccountEditModel accountEditModel, string userId);

        IList<AccountListModel> GetAccountsByUser(string userId);

        IList<AccountForMenuModel> GetAccountsByUserForMenu(string userId);

        AccountEditModel GetById(int id);

        void EditBankAccount(AccountEditModel accountEditModel, string userId);

        void DeleteBankAccount(int id);
    }
}