using System;
using System.Collections.Generic;
using System.Linq;
using PFM.DataAccessLayer.Entities;
using AutoMapper;
using PFM.Services.Interfaces;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.MovementStrategy;
using PFM.Api.Contracts.Saving;
using PFM.Services.Events.Interfaces;
using System.Transactions;
using System.Threading.Tasks;
using PFM.Services.Caches.Interfaces;

namespace PFM.Services
{
    public class SavingService : ISavingService
    {
        private readonly ISavingRepository _savingRepository;
        private readonly IBankAccountCache _bankAccountCache;
        private readonly IIncomeRepository _incomeRepository;
        private readonly IAtmWithdrawRepository _atmWithdrawRepository;
        private readonly IEventPublisher _eventPublisher;

        public SavingService(ISavingRepository savingRepository, IBankAccountCache bankAccountCache, 
             IIncomeRepository incomeRepository, IAtmWithdrawRepository atmWithdrawRepository, IEventPublisher eventPublisher)
        {
            this._savingRepository = savingRepository;
            this._bankAccountCache = bankAccountCache;
            this._incomeRepository = incomeRepository;
            this._atmWithdrawRepository = atmWithdrawRepository;
            this._eventPublisher = eventPublisher;
        }

        public async Task<bool> CreateSaving(SavingDetails savingDetails)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var saving = Mapper.Map<Saving>(savingDetails);

                var movement = new Movement(savingDetails);

                var strategy = ContextMovementStrategy.GetMovementStrategy(movement, _bankAccountCache, _incomeRepository, _atmWithdrawRepository, _eventPublisher);
                var result  = await strategy.Debit();

                if (!movement.TargetIncomeId.HasValue)
                    throw new ArgumentException("Target Income ID should not be null.");

                saving.GeneratedIncomeId = movement.TargetIncomeId.Value;

                _savingRepository.Create(saving);

                scope.Complete();

                return result;
            }
        }

        public async Task<bool> DeleteSaving(int id)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var saving = _savingRepository.GetById(id);
                var savingDetails = Mapper.Map<SavingDetails>(saving);

                _savingRepository.Delete(saving);

                var strategy = ContextMovementStrategy.GetMovementStrategy(new Movement(savingDetails), _bankAccountCache, _incomeRepository, _atmWithdrawRepository, _eventPublisher);
                var result = await strategy.Credit();

                scope.Complete();

                return result;
            }
        }

        public async Task<SavingDetails> GetById(int id)
        {
            var saving = _savingRepository.GetById(id);

            if (saving == null)
            {
                return null;
            }

            var mappedSaving = Mapper.Map<SavingDetails>(saving);

            var targetAccount = await _bankAccountCache.GetById(saving.TargetInternalAccountId);
            mappedSaving.TargetInternalAccountName = targetAccount.Name;

            return mappedSaving;
        }

        public async Task<IList<SavingList>> GetSavingsByAccountId(int accountId)
        {
            var savings = _savingRepository.GetList2().Where(x => x.AccountId == accountId).ToList();

            var mappedSavings = new List<SavingList>();

            foreach (var saving in savings)
            {
                var map = Mapper.Map<SavingList>(saving);

                var srcAccount = await _bankAccountCache.GetById(saving.AccountId);
                map.AccountCurrencySymbol = srcAccount.CurrencySymbol;

                var targetAccount = await _bankAccountCache.GetById(saving.TargetInternalAccountId);
                map.TargetInternalAccountName = targetAccount.Name;

                mappedSavings.Add(map);
            }

            return mappedSavings.ToList();
        }
    }
}