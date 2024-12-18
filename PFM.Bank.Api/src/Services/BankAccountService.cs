﻿using AutoMapper;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interfaces;
using PFM.Bank.Api.Contracts.Account;
using PFM.Bank.Event.Contracts;
using PFM.Services.Core.Exceptions;
using Services.Events.Interfaces;
using Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Services
{
    public class BankAccountService: IBankAccountService
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IEventPublisher _eventPublisher;

        private readonly string PropertyCannotBeModified = "Property cannot be modified on an existing bank account.";

        public BankAccountService(IBankAccountRepository bankAccountRepository, IEventPublisher eventPublisher)
        {
            this._bankAccountRepository = bankAccountRepository;
            this._eventPublisher = eventPublisher;
        }

        public async Task<bool> CreateBankAccount(AccountDetails accountDetails, string userId)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var account = Mapper.Map<Account>(accountDetails);

                account.User_Id = userId;
                account.CurrentBalance = account.InitialBalance;
                account.IsFavorite = !_bankAccountRepository.GetList().Any(x => x.User_Id == userId);

                _bankAccountRepository.Create(account);

                var evt = Mapper.Map<BankAccountCreated>(account);

                var published = await _eventPublisher.PublishAsync(evt, default);
                
                scope.Complete();

                return published;
            }
        }

        public Task<List<AccountList>> GetAccountsByUser(string userId)
        {
            var accounts = _bankAccountRepository
                .GetList2(u => u.Currency, u => u.Bank)
                .Where(x => x.User_Id == userId)
                .ToList();

            var mappedAccounts = accounts.Select(x => Mapper.Map<AccountList>(x)).ToList();

            mappedAccounts.ForEach(account =>
            {
                // TODO: If we ever need some control over this, we need an endpoint to call to check if any movements were done already.
                account.CanBeDeleted = false;
            });

            return Task.FromResult(mappedAccounts);
        }
        
        public Task<AccountDetails> GetById(int id)
        {
            var account = _bankAccountRepository.GetList2(x => x.Currency, x => x.Bank).SingleOrDefault(x => x.Id == id);

            if (account == null)
            {
                return null;
            }

            return Task.FromResult(Mapper.Map<AccountDetails>(account));
        }

        public Task<bool> EditBankAccount(AccountDetails accountDetails, string userId)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var account = _bankAccountRepository.GetById(accountDetails.Id, a => a.Currency, a => a.Bank);

                var businessException = new BusinessException();

                if (account.Currency.Id != accountDetails.CurrencyId)
                {
                    businessException.AddErrorMessage(nameof(account.Currency), PropertyCannotBeModified);
                }

                if (account.Bank.Id != accountDetails.BankId)
                {
                    businessException.AddErrorMessage(nameof(account.Bank), PropertyCannotBeModified);
                }

                if (account.User_Id != userId)
                {
                    businessException.AddErrorMessage(nameof(account.User_Id), PropertyCannotBeModified);
                }

                if (account.IsSavingAccount != accountDetails.IsSavingAccount)
                {
                    businessException.AddErrorMessage(nameof(account.IsSavingAccount), PropertyCannotBeModified);
                }

                if (businessException.HasError())
                    throw businessException;

                account = Mapper.Map<Account>(accountDetails);
                account.User_Id = userId;
                _bankAccountRepository.Update(account);

                var updated = _bankAccountRepository.GetById(accountDetails.Id, a => a.Currency, a => a.Bank);

                scope.Complete();

                return Task.FromResult(true);
            }
        }

        public async Task<bool> DeleteBankAccount(int id)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var account = _bankAccountRepository.GetById(id, a => a.Currency, a => a.Bank);
                _bankAccountRepository.Delete(account);

                var evt = Mapper.Map<BankAccountDeleted>(account);

                var published = await _eventPublisher.PublishAsync(evt, default);

                scope.Complete();

                return published;
            }
        }
        
        public Task<bool> SetAsFavorite(int id)
        {
            var updatedList = _bankAccountRepository.GetList2();

            foreach (var account in updatedList)
            {
                account.IsFavorite = account.Id == id;
            }

            _bankAccountRepository.UpdateAll(updatedList);

            return Task.FromResult(true);
        }
    }
}