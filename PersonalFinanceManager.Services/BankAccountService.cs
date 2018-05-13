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
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;

namespace PersonalFinanceManager.Services
{
    public class BankAccountService: IBankAccountService
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IExpenditureRepository _expenditureRepository;
        private readonly IIncomeRepository _incomeRepository;
        private readonly IAtmWithdrawRepository _atmWithdrawRepository;
        private readonly IBankBranchRepository _bankBranchRepository;

        public BankAccountService(IBankAccountRepository bankAccountRepository, IExpenditureRepository expenditureRepository, IIncomeRepository incomeRepository,
            IAtmWithdrawRepository atmWithdrawRepository, IBankBranchRepository bankBranchRepository)
        {
            this._bankAccountRepository = bankAccountRepository;
            this._expenditureRepository = expenditureRepository;
            this._incomeRepository = incomeRepository;
            this._atmWithdrawRepository = atmWithdrawRepository;
            this._bankBranchRepository = bankBranchRepository;
        }

        public void CreateBankAccount(AccountEditModel accountEditModel, string userId)
        {
            var accountModel = Mapper.Map<AccountModel>(accountEditModel);

            var user = new ApplicationUser() { Id = userId };

            accountModel.User_Id = user.Id;
            accountModel.CurrentBalance = accountModel.InitialBalance;
            accountModel.IsFavorite = !_bankAccountRepository.GetList().Any(x => x.User_Id == userId);

            _bankAccountRepository.Create(accountModel);
        }

        public IList<AccountListModel> GetAccountsByUser(string userId)
        {
            var accounts = _bankAccountRepository.GetList()
                .Include(u => u.Currency)
                .Include(u => u.Bank)
                .Where(x => x.User_Id == userId)
                .ToList();

            var accountsModel = accounts.Select(x => Mapper.Map<AccountListModel>(x)).ToList();

            accountsModel.ForEach(account =>
            {
                var hasExpenditures = _expenditureRepository.GetList().Any(x => x.AccountId == account.Id);
                var hasIncome = _incomeRepository.GetList().Any(x => x.AccountId == account.Id);
                var hasAtmWithdraw = _atmWithdrawRepository.GetList().Any(x => x.AccountId == account.Id);

                account.CanBeDeleted = !hasExpenditures && !hasIncome && !hasAtmWithdraw;
            });

            return accountsModel;
        }
        
        public AccountEditModel GetById(int id)
        {
            var account = _bankAccountRepository.GetList().Include(x => x.Currency).Include(x => x.Bank).SingleOrDefault(x => x.Id == id);

            if (account == null)
            {
                return null;
            }

            // Can be null for online banking
            var favoriteBankDetails = _bankBranchRepository.GetList().SingleOrDefault(x => x.BankId == account.BankId);

            var accountModel = Mapper.Map<AccountEditModel>(account);

            if (favoriteBankDetails != null)
            {
                accountModel.BankBranchName = favoriteBankDetails.Name;
                accountModel.BankBranchAddressLine1 = favoriteBankDetails.AddressLine1;
                accountModel.BankBranchAddressLine2 = favoriteBankDetails.AddressLine2;
                accountModel.BankBranchPostCode = favoriteBankDetails.PostCode;
                accountModel.BankBranchCity = favoriteBankDetails.City;
                accountModel.BankBranchPhoneNumber = favoriteBankDetails.PhoneNumber;
            }
            return accountModel;
        }

        public void EditBankAccount(AccountEditModel accountEditModel, string userId)
        {
            var accountModel = _bankAccountRepository.GetList().AsNoTracking().SingleOrDefault(x => x.Id == accountEditModel.Id);
            accountModel = Mapper.Map<AccountModel>(accountEditModel);
            accountModel.User_Id = userId;
            _bankAccountRepository.Update(accountModel);
        }

        public void DeleteBankAccount(int id)
        {
            var accountModel = _bankAccountRepository.GetById(id);
            _bankAccountRepository.Delete(accountModel);
        }
        
        public void SetAsFavorite(int id)
        {
            foreach(var account in _bankAccountRepository.GetList())
            {
                account.IsFavorite = account.Id == id;
                _bankAccountRepository.Update(account);
            }
        }
    }
}