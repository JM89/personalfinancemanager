using AutoMapper;
using PFM.Api.Contracts.AtmWithdraw;
using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Enumerations;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.Events.EventTypes;
using PFM.Services.Events.Interfaces;
using PFM.Services.Helpers;
using PFM.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace PFM.Services
{
    public class AtmWithdrawService : IAtmWithdrawService
    {
        private readonly IAtmWithdrawRepository _atmWithdrawRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IExpenseRepository _expenditureRepository;
        private readonly IHistoricMovementRepository _historicMovementRepository;
        private readonly IEventPublisher _eventPublisher;

        private readonly string OperationType = "ATM Withdrawal";

        public AtmWithdrawService(IAtmWithdrawRepository atmWithdrawRepository, IBankAccountRepository bankAccountRepository, IExpenseRepository expenditureRepository,
            IHistoricMovementRepository historicMovementRepository, IEventPublisher eventPublisher)
        {
            this._atmWithdrawRepository = atmWithdrawRepository;
            this._bankAccountRepository = bankAccountRepository;
            this._expenditureRepository = expenditureRepository;
            this._historicMovementRepository = historicMovementRepository;
            this._eventPublisher = eventPublisher;
        }
              
        public IList<AtmWithdrawList> GetAtmWithdrawsByAccountId(int accountId)
        {
            var atmWithdraws = _atmWithdrawRepository.GetList2(u => u.Account.Currency)
                .Where(x => x.Account.Id == accountId)
                .ToList();

            var expenditures = _expenditureRepository.GetList();

            var mappedAtmWithdraws = atmWithdraws.Select(Mapper.Map<AtmWithdrawList>).ToList();

            mappedAtmWithdraws.ForEach(atmWithdraw =>
            {
                atmWithdraw.CanBeDeleted = !expenditures.Any(x => x.AtmWithdrawId == atmWithdraw.Id);
                atmWithdraw.CanBeEdited = !expenditures.Any(x => x.AtmWithdrawId == atmWithdraw.Id); 
            });

            return mappedAtmWithdraws;
        }

        public Task<bool> CreateAtmWithdraws(List<AtmWithdrawDetails> atmWithdrawDetails)
        {
            var resultBatch = true;
            atmWithdrawDetails.ForEach(async (income) => {
                var result = await CreateAtmWithdraw(income);
                if (!result)
                    resultBatch = false;
            });
            return Task.FromResult(resultBatch);
        }

        public async Task<bool> CreateAtmWithdraw(AtmWithdrawDetails atmWithdrawDetails)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var atmWithdraw = Mapper.Map<AtmWithdraw>(atmWithdrawDetails);
                atmWithdraw.CurrentAmount = atmWithdrawDetails.InitialAmount;
                atmWithdraw.IsClosed = false;
                _atmWithdrawRepository.Create(atmWithdraw);

                var account = _bankAccountRepository.GetById(atmWithdraw.AccountId, a => a.Currency, a => a.Bank);

                var evt = new BankAccountDebited()
                {
                    BankCode = account.Bank.Id.ToString(),
                    CurrencyCode = account.Currency.Id.ToString(),
                    PreviousBalance = account.CurrentBalance,
                    CurrentBalance = account.CurrentBalance - atmWithdraw.InitialAmount,
                    UserId = account.User_Id,
                    OperationDate = atmWithdraw.DateExpense,
                    OperationType = OperationType
                };

                MovementHelpers.Debit(_historicMovementRepository, atmWithdraw.InitialAmount, account.Id, ObjectType.Account, account.CurrentBalance);

                account.CurrentBalance -= atmWithdraw.InitialAmount;
                _bankAccountRepository.Update(account);

                var published = await _eventPublisher.PublishAsync(evt, default);

                scope.Complete();

                return published;
            }
        }

        public AtmWithdrawDetails GetById(int id)
        {
            var atmWithdraw = _atmWithdrawRepository.GetById(id);

            if (atmWithdraw == null)
            {
                return null;
            }

            return Mapper.Map<AtmWithdrawDetails>(atmWithdraw);
        }

        public async Task<bool> EditAtmWithdraw(AtmWithdrawDetails atmWithdrawDetails)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var atmWithdraw = _atmWithdrawRepository.GetById(atmWithdrawDetails.Id);

                var oldCost = atmWithdraw.InitialAmount;

                atmWithdraw.InitialAmount = atmWithdrawDetails.InitialAmount;
                atmWithdraw.CurrentAmount = atmWithdrawDetails.InitialAmount;
                atmWithdraw.DateExpense = atmWithdrawDetails.DateExpense;
                atmWithdraw.HasBeenAlreadyDebited = atmWithdrawDetails.HasBeenAlreadyDebited;

                _atmWithdrawRepository.Update(atmWithdraw);

                var published = true;
                if (oldCost != atmWithdraw.InitialAmount)
                {
                    var account = _bankAccountRepository.GetById(atmWithdraw.AccountId, a => a.Currency, a => a.Bank);
                    MovementHelpers.Credit(_historicMovementRepository, oldCost, account.Id, ObjectType.Account, account.CurrentBalance);

                    var evtCredited = new BankAccountCredited()
                    {
                        BankCode = account.Bank.Id.ToString(),
                        CurrencyCode = account.Currency.Id.ToString(),
                        PreviousBalance = account.CurrentBalance,
                        CurrentBalance = account.CurrentBalance + oldCost,
                        UserId = account.User_Id,
                        OperationDate = atmWithdraw.DateExpense,
                        OperationType = OperationType
                    };

                    account.CurrentBalance += oldCost;
                    MovementHelpers.Debit(_historicMovementRepository, atmWithdraw.InitialAmount, account.Id, ObjectType.Account, account.CurrentBalance);

                    var evtDebited = new BankAccountDebited()
                    {
                        BankCode = account.Bank.Id.ToString(),
                        CurrencyCode = account.Currency.Id.ToString(),
                        PreviousBalance = account.CurrentBalance,
                        CurrentBalance = account.CurrentBalance - atmWithdraw.InitialAmount,
                        UserId = account.User_Id,
                        OperationDate = atmWithdraw.DateExpense,
                        OperationType = OperationType
                    };

                    account.CurrentBalance -= atmWithdraw.InitialAmount;
                    _bankAccountRepository.Update(account);

                    published = await _eventPublisher.PublishAsync(evtDebited, default);
                    if (published)
                        published = await _eventPublisher.PublishAsync(evtCredited, default);
                }

                scope.Complete();

                return published;
            }
        }
        
        public void CloseAtmWithdraw(int id)
        {
            var atmWithdraw = _atmWithdrawRepository.GetById(id); 
            atmWithdraw.IsClosed = true;
            _atmWithdrawRepository.Update(atmWithdraw);
        }

        public async Task<bool> DeleteAtmWithdraw(int id)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var atmWithdraw = _atmWithdrawRepository.GetById(id);

                var account = _bankAccountRepository.GetById(atmWithdraw.AccountId, a => a.Currency, a => a.Bank);
                MovementHelpers.Credit(_historicMovementRepository, atmWithdraw.InitialAmount, account.Id, ObjectType.Account, account.CurrentBalance);

                var evt = new BankAccountCredited()
                {
                    BankCode = account.Bank.Id.ToString(),
                    CurrencyCode = account.Currency.Id.ToString(),
                    PreviousBalance = account.CurrentBalance,
                    CurrentBalance = account.CurrentBalance + atmWithdraw.InitialAmount,
                    UserId = account.User_Id,
                    OperationDate = atmWithdraw.DateExpense,
                    OperationType = OperationType
                };

                account.CurrentBalance += atmWithdraw.InitialAmount;
                _bankAccountRepository.Update(account);

                _atmWithdrawRepository.Delete(atmWithdraw);

                var published = await _eventPublisher.PublishAsync(evt, default);

                scope.Complete();

                return published;
            }
        }

        public void ChangeDebitStatus(int id, bool debitStatus)
        {
            AtmWithdraw atmWithdraw = _atmWithdrawRepository.GetById(id);
            atmWithdraw.HasBeenAlreadyDebited = debitStatus;
            _atmWithdrawRepository.Update(atmWithdraw);
        }
    }
}