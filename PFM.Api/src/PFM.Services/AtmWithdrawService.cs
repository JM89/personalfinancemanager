using AutoMapper;
using PFM.Api.Contracts.AtmWithdraw;
using PFM.Bank.Event.Contracts;
using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.Caches.Interfaces;
using PFM.Services.Events.Interfaces;
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
        private readonly IBankAccountCache _bankAccountCache;
        private readonly IExpenseRepository _expenditureRepository;
        private readonly IEventPublisher _eventPublisher;

        private readonly string OperationType = "ATM Withdrawal";

        public AtmWithdrawService(IAtmWithdrawRepository atmWithdrawRepository, IBankAccountCache bankAccountCache, IExpenseRepository expenditureRepository,
            IEventPublisher eventPublisher)
        {
            this._atmWithdrawRepository = atmWithdrawRepository;
            this._bankAccountCache = bankAccountCache;
            this._expenditureRepository = expenditureRepository;
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

                var account = await _bankAccountCache.GetById(atmWithdraw.AccountId);

                var evt = new BankAccountDebited()
                {
                    Id = account.Id,
                    BankId = account.BankId,
                    CurrencyId = account.CurrencyId,
                    PreviousBalance = account.CurrentBalance,
                    CurrentBalance = account.CurrentBalance - atmWithdraw.InitialAmount,
                    UserId = account.UserId,
                    OperationDate = atmWithdraw.DateExpense,
                    OperationType = OperationType
                };

                account.CurrentBalance -= atmWithdraw.InitialAmount;

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

                var account = await _bankAccountCache.GetById(atmWithdraw.AccountId);

                var evt = new BankAccountCredited()
                {
                    Id = account.Id,
                    BankId = account.BankId,
                    CurrencyId = account.CurrencyId,
                    PreviousBalance = account.CurrentBalance,
                    CurrentBalance = account.CurrentBalance + atmWithdraw.InitialAmount,
                    UserId = account.UserId,
                    OperationDate = atmWithdraw.DateExpense,
                    OperationType = OperationType
                };

                account.CurrentBalance += atmWithdraw.InitialAmount;

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