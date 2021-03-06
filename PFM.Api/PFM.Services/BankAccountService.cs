﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PFM.Services.Interfaces;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;
using PFM.Services.DTOs.Account;

namespace PFM.Services
{
    public class BankAccountService: IBankAccountService
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IExpenseRepository _expenditureRepository;
        private readonly IIncomeRepository _incomeRepository;
        private readonly IAtmWithdrawRepository _atmWithdrawRepository;
        private readonly IBankBranchRepository _bankBranchRepository;

        public BankAccountService(IBankAccountRepository bankAccountRepository, IExpenseRepository expenditureRepository, IIncomeRepository incomeRepository,
            IAtmWithdrawRepository atmWithdrawRepository, IBankBranchRepository bankBranchRepository)
        {
            this._bankAccountRepository = bankAccountRepository;
            this._expenditureRepository = expenditureRepository;
            this._incomeRepository = incomeRepository;
            this._atmWithdrawRepository = atmWithdrawRepository;
            this._bankBranchRepository = bankBranchRepository;
        }

        public void CreateBankAccount(AccountDetails accountDetails, string userId)
        {
            var account = Mapper.Map<Account>(accountDetails);

            account.User_Id = userId;
            account.CurrentBalance = account.InitialBalance;
            account.IsFavorite = !_bankAccountRepository.GetList().Any(x => x.User_Id == userId);

            _bankAccountRepository.Create(account);
        }

        public IList<AccountList> GetAccountsByUser(string userId)
        {
            var accounts = _bankAccountRepository.GetList2(u => u.Currency, u => u.Bank)
                .Where(x => x.User_Id == userId)
                .ToList();

            var mappedAccounts = accounts.Select(x => Mapper.Map<AccountList>(x)).ToList();

            mappedAccounts.ForEach(account =>
            {
                var hasExpenditures = _expenditureRepository.GetList().Any(x => x.AccountId == account.Id);
                var hasIncome = _incomeRepository.GetList().Any(x => x.AccountId == account.Id);
                var hasAtmWithdraw = _atmWithdrawRepository.GetList().Any(x => x.AccountId == account.Id);

                account.CanBeDeleted = !hasExpenditures && !hasIncome && !hasAtmWithdraw;
            });

            return mappedAccounts;
        }
        
        public AccountDetails GetById(int id)
        {
            var account = _bankAccountRepository.GetList2(x => x.Currency, x => x.Bank).SingleOrDefault(x => x.Id == id);

            if (account == null)
            {
                return null;
            }

            // Can be null for online banking
            var favoriteBankDetails = _bankBranchRepository.GetList().SingleOrDefault(x => x.BankId == account.BankId);

            var mappedAccount = Mapper.Map<AccountDetails>(account);

            if (favoriteBankDetails != null)
            {
                mappedAccount.BankBranchName = favoriteBankDetails.Name;
                mappedAccount.BankBranchAddressLine1 = favoriteBankDetails.AddressLine1;
                mappedAccount.BankBranchAddressLine2 = favoriteBankDetails.AddressLine2;
                mappedAccount.BankBranchPostCode = favoriteBankDetails.PostCode;
                mappedAccount.BankBranchCity = favoriteBankDetails.City;
                mappedAccount.BankBranchPhoneNumber = favoriteBankDetails.PhoneNumber;
            }
            return mappedAccount;
        }

        public void EditBankAccount(AccountDetails accountDetails, string userId)
        {
            var account = _bankAccountRepository.GetListAsNoTracking().SingleOrDefault(x => x.Id == accountDetails.Id);
            account = Mapper.Map<Account>(accountDetails);
            account.User_Id = userId;
            _bankAccountRepository.Update(account);
        }

        public void DeleteBankAccount(int id)
        {
            var account = _bankAccountRepository.GetById(id);
            _bankAccountRepository.Delete(account);
        }
        
        public void SetAsFavorite(int id)
        {
            var updatedList = _bankAccountRepository.GetList2();

            foreach (var account in updatedList)
            {
                account.IsFavorite = account.Id == id;
            }

            _bankAccountRepository.UpdateAll(updatedList);
        }
    }
}