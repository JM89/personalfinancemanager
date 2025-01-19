using AutoMapper;
using DataAccessLayer.Entities;
using PFM.Bank.Api.Contracts.Account;
using PFM.Bank.Event.Contracts;
using PFM.Services.Core.Exceptions;
using Services.Events.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using DataAccessLayer.Repositories;
using Services.Core;

namespace Services
{
    public interface IBankAccountService : IBaseService
    {
        Task<bool> CreateBankAccount(AccountDetails accountDetails, string userId);

        Task<List<AccountList>> GetAccountsByUser(string userId);

        Task<AccountDetails> GetById(int id);

        Task<bool> EditBankAccount(AccountDetails accountDetails, string userId);

        Task<bool> DeleteBankAccount(int id);

        Task<bool> SetAsFavorite(int id);
    }
    
    public class BankAccountService(IMapper mapper, IBankAccountRepository repository, IEventPublisher eventPublisher)
        : IBankAccountService
    {
        private const string PropertyCannotBeModified = "Property cannot be modified on an existing bank account.";

        public async Task<bool> CreateBankAccount(AccountDetails accountDetails, string userId)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var account = mapper.Map<Account>(accountDetails);

                account.User_Id = userId;
                account.CurrentBalance = account.InitialBalance;
                account.IsFavorite = !repository.GetList().Any(x => x.User_Id == userId);

                repository.Create(account);

                var evt = mapper.Map<BankAccountCreated>(account);

                var published = await eventPublisher.PublishAsync(evt, default);
                
                scope.Complete();

                return published;
            }
        }

        public Task<List<AccountList>> GetAccountsByUser(string userId)
        {
            var accounts = repository
                .GetList2(u => u.Currency, u => u.Bank)
                .Where(x => x.User_Id == userId)
                .ToList();

            var mappedAccounts = accounts.Select(mapper.Map<AccountList>).ToList();

            mappedAccounts.ForEach(account =>
            {
                // TODO: If we ever need some control over this, we need an endpoint to call to check if any movements were done already.
                account.CanBeDeleted = false;
            });

            return Task.FromResult(mappedAccounts);
        }
        
        public Task<AccountDetails> GetById(int id)
        {
            var account = repository.GetList2(x => x.Currency, x => x.Bank).SingleOrDefault(x => x.Id == id);

            if (account == null)
            {
                return null;
            }

            return Task.FromResult(mapper.Map<AccountDetails>(account));
        }

        public Task<bool> EditBankAccount(AccountDetails accountDetails, string userId)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            var account = repository.GetById(accountDetails.Id, a => a.Currency, a => a.Bank);

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

            account = mapper.Map<Account>(accountDetails);
            account.User_Id = userId;
            repository.Update(account);

            var updated = repository.GetById(accountDetails.Id, a => a.Currency, a => a.Bank);

            scope.Complete();

            return Task.FromResult(true);
        }

        public async Task<bool> DeleteBankAccount(int id)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var account = repository.GetById(id, a => a.Currency, a => a.Bank);
                repository.Delete(account);

                var evt = mapper.Map<BankAccountDeleted>(account);

                var published = await eventPublisher.PublishAsync(evt, default);

                scope.Complete();

                return published;
            }
        }
        
        public Task<bool> SetAsFavorite(int id)
        {
            var updatedList = repository.GetList2();

            foreach (var account in updatedList)
            {
                account.IsFavorite = account.Id == id;
            }

            repository.UpdateAll(updatedList);

            return Task.FromResult(true);
        }
    }
}