using AutoMapper;
using PFM.Api.Contracts.Account;
using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.Events.EventTypes;
using PFM.Services.Events.Interfaces;
using PFM.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace PFM.Services
{
    public class BankAccountService: IBankAccountService
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IExpenseRepository _expenditureRepository;
        private readonly IIncomeRepository _incomeRepository;
        private readonly IAtmWithdrawRepository _atmWithdrawRepository;
        private readonly IBankBranchRepository _bankBranchRepository;
        private readonly IEventPublisher _eventPublisher;

        public BankAccountService(IBankAccountRepository bankAccountRepository, IExpenseRepository expenditureRepository, IIncomeRepository incomeRepository,
            IAtmWithdrawRepository atmWithdrawRepository, IBankBranchRepository bankBranchRepository, IEventPublisher eventPublisher)
        {
            this._bankAccountRepository = bankAccountRepository;
            this._expenditureRepository = expenditureRepository;
            this._incomeRepository = incomeRepository;
            this._atmWithdrawRepository = atmWithdrawRepository;
            this._bankBranchRepository = bankBranchRepository;
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

                var added = _bankAccountRepository.Create(account);

                added = _bankAccountRepository.GetById(added.Id, a => a.Currency, a => a.Bank);

                var evt = new BankAccountCreated() { 
                    BankCode = added.Id.ToString(), 
                    CurrencyCode = added.Currency.Id.ToString(), 
                    CurrentBalance = added.CurrentBalance,
                    UserId = added.User_Id
                };

                var published = await _eventPublisher.PublishAsync(evt, default);
                
                scope.Complete();

                return published;
            }
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

        public async Task<bool> EditBankAccount(AccountDetails accountDetails, string userId)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var account = _bankAccountRepository.GetListAsNoTracking().SingleOrDefault(x => x.Id == accountDetails.Id);
                account = Mapper.Map<Account>(accountDetails);
                account.User_Id = userId;
                _bankAccountRepository.Update(account);

                var updated = _bankAccountRepository.GetById(accountDetails.Id, a => a.Currency, a => a.Bank);

                var evt = new BankAccountUpdated()
                {
                    BankCode = updated.Id.ToString(),
                    CurrencyCode = updated.Currency.Id.ToString(),
                    CurrentBalance = updated.CurrentBalance,
                    UserId = updated.User_Id
                };

                var published = await _eventPublisher.PublishAsync(evt, default);

                scope.Complete();

                return published;
            }
        }

        public async Task<bool> DeleteBankAccount(int id)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var account = _bankAccountRepository.GetById(id, a => a.Currency, a => a.Bank);
                _bankAccountRepository.Delete(account);

                var evt = new BankAccountDeleted()
                {
                    BankCode = account.Id.ToString(),
                    CurrencyCode = account.Currency.Id.ToString(),
                    CurrentBalance = account.CurrentBalance,
                    UserId = account.User_Id
                };

                var published = await _eventPublisher.PublishAsync(evt, default);

                scope.Complete();

                return published;
            }
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