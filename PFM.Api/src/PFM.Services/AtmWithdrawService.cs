using AutoMapper;
using PFM.Api.Contracts.AtmWithdraw;
using PFM.Bank.Event.Contracts;
using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.Events.Interfaces;
using PFM.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using PFM.Services.Caches;

namespace PFM.Services
{
    public class AtmWithdrawService(
        IMapper mapper,
        IAtmWithdrawRepository atmWithdrawRepository,
        IBankAccountCache bankAccountCache,
        IExpenseRepository expenditureRepository,
        IEventPublisher eventPublisher)
        : IAtmWithdrawService
    {
        private const string OperationType = "ATM Withdrawal";

        public async Task<IList<AtmWithdrawList>> GetAtmWithdrawsByAccountId(int accountId)
        {
            var atmWithdraws = atmWithdrawRepository.GetList2().Where(x => x.AccountId == accountId).ToList();

            var expenditures = expenditureRepository.GetList();

            var mappedAtmWithdraws = new List<AtmWithdrawList>();

            foreach (var atmWithdraw in atmWithdraws)
            {
                var map = mapper.Map<AtmWithdrawList>(atmWithdraw);

                var account = await bankAccountCache.GetById(atmWithdraw.AccountId);

                map.AccountCurrencySymbol = account.CurrencySymbol;
                map.CanBeDeleted = !expenditures.Any(x => x.AtmWithdrawId == atmWithdraw.Id);
                map.CanBeEdited = !expenditures.Any(x => x.AtmWithdrawId == atmWithdraw.Id);

                mappedAtmWithdraws.Add(map);
            }

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
                var atmWithdraw = mapper.Map<AtmWithdraw>(atmWithdrawDetails);
                atmWithdraw.CurrentAmount = atmWithdrawDetails.InitialAmount;
                atmWithdraw.IsClosed = false;
                atmWithdrawRepository.Create(atmWithdraw);

                var account = await bankAccountCache.GetById(atmWithdraw.AccountId);

                var evt = new BankAccountDebited()
                {
                    Id = account.Id,
                    BankId = account.BankId,
                    CurrencyId = account.CurrencyId,
                    PreviousBalance = account.CurrentBalance,
                    CurrentBalance = account.CurrentBalance - atmWithdraw.InitialAmount,
                    UserId = account.OwnerId,
                    OperationDate = atmWithdraw.DateExpense,
                    OperationType = OperationType
                };

                account.CurrentBalance -= atmWithdraw.InitialAmount;

                var published = await eventPublisher.PublishAsync(evt, default);

                scope.Complete();

                return published;
            }
        }

        public Task<AtmWithdrawDetails> GetById(int id)
        {
            var atmWithdraw = atmWithdrawRepository.GetById(id);

            if (atmWithdraw == null)
            {
                return null;
            }

            return Task.FromResult(mapper.Map<AtmWithdrawDetails>(atmWithdraw));
        }
                
        public Task<bool> CloseAtmWithdraw(int id)
        {
            var atmWithdraw = atmWithdrawRepository.GetById(id); 
            atmWithdraw.IsClosed = true;
            atmWithdrawRepository.Update(atmWithdraw);
            return Task.FromResult(true);
        }

        public async Task<bool> DeleteAtmWithdraw(int id)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var atmWithdraw = atmWithdrawRepository.GetById(id);

                var account = await bankAccountCache.GetById(atmWithdraw.AccountId);

                var evt = new BankAccountCredited()
                {
                    Id = account.Id,
                    BankId = account.BankId,
                    CurrencyId = account.CurrencyId,
                    PreviousBalance = account.CurrentBalance,
                    CurrentBalance = account.CurrentBalance + atmWithdraw.InitialAmount,
                    UserId = account.OwnerId,
                    OperationDate = atmWithdraw.DateExpense,
                    OperationType = OperationType
                };

                account.CurrentBalance += atmWithdraw.InitialAmount;

                atmWithdrawRepository.Delete(atmWithdraw);

                var published = await eventPublisher.PublishAsync(evt, default);

                scope.Complete();

                return published;
            }
        }

        public Task<bool> ChangeDebitStatus(int id, bool debitStatus)
        {
            AtmWithdraw atmWithdraw = atmWithdrawRepository.GetById(id);
            atmWithdraw.HasBeenAlreadyDebited = debitStatus;
            atmWithdrawRepository.Update(atmWithdraw);
            return Task.FromResult(true);
        }
    }
}