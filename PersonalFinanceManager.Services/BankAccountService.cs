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
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Services
{
    public class BankAccountService: IDisposable, IBankAccountService
    {
        ApplicationDbContext db;

        public BankAccountService()
        {
            db = new ApplicationDbContext();
        }

        public void CreateBankAccount(AccountEditModel accountEditModel, string userId)
        {
            var accountModel = Mapper.Map<AccountModel>(accountEditModel);

            var user = new ApplicationUser() { Id = userId };

            accountModel.User_Id = user.Id;
            accountModel.CurrentBalance = accountModel.InitialBalance;

            db.AccountModels.Add(accountModel);
            db.SaveChanges();
        }

        public IList<AccountListModel> GetAccountsByUser(string userId)
        {
            var accounts = db.AccountModels
                .Include(u => u.Currency)
                .Include(u => u.Bank)
                .Where(x => x.User_Id == userId)
                .ToList();

            var accountsModel = accounts.Select(x => Mapper.Map<AccountListModel>(x)).ToList();

            accountsModel.ForEach(account =>
            {
                var hasExpenditures = db.ExpenditureModels.Any(x => x.AccountId == account.Id);
                var hasIncome = db.IncomeModels.Any(x => x.AccountId == account.Id);
                var hasAtmWithdraw = db.AtmWithdrawModels.Any(x => x.AccountId == account.Id);

                account.CanBeDeleted = !hasExpenditures && !hasIncome && !hasAtmWithdraw;
            });

            return accountsModel;
        }

        public IList<AccountForMenuModel> GetAccountsByUserForMenu(string userId)
        {
            var accounts = db.AccountModels.Include(u => u.Bank)
                             .Where(x => x.User_Id == userId).ToList();

            var expenditures = db.ExpenditureModels.Include(u => u.Account);

            var accountsModel = accounts.Select(x => new AccountForMenuModel()
                                 {
                                     Name = x.Name,
                                     Id = x.Id,
                                     BankIcon = x.Bank.IconPath,
                                     HasExpenditures = expenditures.Any(y => y.Account.Id == x.Id)
                                 });

            return accountsModel.ToList();
        }

        public AccountEditModel GetById(int id)
        {
            var account = db.AccountModels.Include(x => x.Currency).SingleOrDefault(x => x.Id == id);

            if (account == null)
            {
                return null;
            }

            return Mapper.Map<AccountEditModel>(account);
        }

        public void EditBankAccount(AccountEditModel accountEditModel, string userId)
        {
            var accountModel = db.AccountModels.SingleOrDefault(x => x.Id == accountEditModel.Id);

            accountModel.Name = accountEditModel.Name;
            accountModel.CurrencyId = accountEditModel.CurrencyId;
            accountModel.BankId = accountEditModel.BankId;

            db.Entry(accountModel).State = EntityState.Modified;

            db.SaveChanges();
        }

        public void DeleteBankAccount(int id)
        {
            AccountModel accountModel = db.AccountModels.Find(id);
            db.AccountModels.Remove(accountModel);
            db.SaveChanges();
        }
        
        public void SetAsFavorite(int id)
        {
            foreach(var account in db.AccountModels)
            {
                account.IsFavorite = account.Id == id;
                db.Entry(account).State = EntityState.Modified;
            }

            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}