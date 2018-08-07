using System;
using System.Collections.Generic;
using System.Linq;
using PFM.DataAccessLayer.Entities;
using AutoMapper;
using PFM.Services.Interfaces;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.MovementStrategy;
using PFM.DTOs.Saving;

namespace PFM.Services
{
    public class SavingService : ISavingService
    {
        private readonly ISavingRepository _savingRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IHistoricMovementRepository _historicMovementRepository;
        private readonly IIncomeRepository _incomeRepository;
        private readonly IAtmWithdrawRepository _atmWithdrawRepository;

        public SavingService(ISavingRepository savingRepository, IBankAccountRepository bankAccountRepository, IHistoricMovementRepository historicMovementRepository,
             IIncomeRepository incomeRepository, IAtmWithdrawRepository atmWithdrawRepository)
        {
            this._savingRepository = savingRepository;
            this._bankAccountRepository = bankAccountRepository;
            this._historicMovementRepository = historicMovementRepository;
            this._incomeRepository = incomeRepository;
            this._atmWithdrawRepository = atmWithdrawRepository;
        }

        public void CreateSaving(SavingDetails savingDetails)
        {
            var saving = Mapper.Map<Saving>(savingDetails);

            var movement = new Movement(savingDetails);

            var strategy = ContextMovementStrategy.GetMovementStrategy(movement, _bankAccountRepository, _historicMovementRepository, _incomeRepository, _atmWithdrawRepository);
            strategy.Debit();

            if (!movement.TargetIncomeId.HasValue)
                throw new ArgumentException("Target Income ID should not be null.");

            saving.GeneratedIncomeId = movement.TargetIncomeId.Value;

            _savingRepository.Create(saving);
        }

        public void DeleteSaving(int id)
        {
            var saving = _savingRepository.GetById(id);
            var savingDetails = Mapper.Map<SavingDetails>(saving);

            _savingRepository.Delete(saving);

            var strategy = ContextMovementStrategy.GetMovementStrategy(new Movement(savingDetails), _bankAccountRepository, _historicMovementRepository, _incomeRepository, _atmWithdrawRepository);
            strategy.Credit();
        }

        public void EditSaving(SavingDetails savingDetails)
        {
            var saving = _savingRepository.GetById(savingDetails.Id, true);

            var oldMovement = new Movement(Mapper.Map<SavingDetails>(saving));

            // Update the saving . Reset of generated income as it might be deleted when UpdateDebit.
            saving = Mapper.Map<Saving>(savingDetails);
            saving.GeneratedIncomeId = (int?)null;
            _savingRepository.Update(saving);

            var strategy = ContextMovementStrategy.GetMovementStrategy(oldMovement, _bankAccountRepository, _historicMovementRepository, _incomeRepository, _atmWithdrawRepository);
            var newMovement = new Movement(savingDetails);

            strategy.UpdateDebit(newMovement);

            if (!newMovement.TargetIncomeId.HasValue)
                throw new ArgumentException("Target Income ID should not be null.");

            // Update the GenerateIncomeId.
            saving.GeneratedIncomeId = newMovement.TargetIncomeId.Value;
            _savingRepository.Update(saving);
        }

        public SavingDetails GetById(int id)
        {
            var saving = _savingRepository.GetById(id, u => u.TargetInternalAccount);

            if (saving == null)
            {
                return null;
            }

            var mappedSaving = Mapper.Map<SavingDetails>(saving);

            return mappedSaving;
        }

        public IList<SavingList> GetSavingsByAccountId(int accountId)
        {
            var savings = _savingRepository.GetList2(u => u.Account.Currency, u => u.TargetInternalAccount)
                 .Where(x => x.Account.Id == accountId)
                 .ToList();

            var mappedSavings = savings.Select(x => Mapper.Map<SavingList>(x));

            return mappedSavings.ToList();
        }
    }
}