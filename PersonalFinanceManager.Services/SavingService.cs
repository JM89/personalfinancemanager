using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using PersonalFinanceManager.Entities;
using AutoMapper;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Models.Saving;
using PersonalFinanceManager.Services.MovementStrategy;

namespace PersonalFinanceManager.Services
{
    public class SavingService : ISavingService
    {
        private readonly ISavingRepository _savingRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IHistoricMovementRepository _historicMovementRepository;
        private readonly IIncomeRepository _incomeRepository;

        public SavingService(ISavingRepository savingRepository, IBankAccountRepository bankAccountRepository, IHistoricMovementRepository historicMovementRepository,
             IIncomeRepository incomeRepository)
        {
            this._savingRepository = savingRepository;
            this._bankAccountRepository = bankAccountRepository;
            this._historicMovementRepository = historicMovementRepository;
            this._incomeRepository = incomeRepository;
        }

        public void CreateSaving(SavingEditModel savingEditModel)
        {
            var savingModel = Mapper.Map<SavingModel>(savingEditModel);

            var movement = new Movement(savingEditModel);

            var strategy = ContextMovementStrategy.GetMovementStrategy(movement, _bankAccountRepository, _historicMovementRepository, _incomeRepository);
            strategy.Debit();

            savingModel.GeneratedIncomeId = movement.TargetIncomeId.Value;

            _savingRepository.Create(savingModel);
        }

        public void DeleteSaving(int id)
        {
            var savingModel = _savingRepository.GetById(id);
            var savingEditModel = Mapper.Map<SavingEditModel>(savingModel);

            _savingRepository.Delete(savingModel);

            var strategy = ContextMovementStrategy.GetMovementStrategy(new Movement(savingEditModel), _bankAccountRepository, _historicMovementRepository, _incomeRepository);
            strategy.Credit();
        }

        public void EditSaving(SavingEditModel savingEditModel)
        {
            var savingModel = _savingRepository.GetById(savingEditModel.Id, true);

            var oldMovement = new Movement(Mapper.Map<SavingEditModel>(savingModel));

            // Update the saving model. Reset of generated income as it might be deleted when UpdateDebit.
            savingModel = Mapper.Map<SavingModel>(savingEditModel);
            savingModel.GeneratedIncomeId = (int?)null;
            _savingRepository.Update(savingModel);

            var strategy = ContextMovementStrategy.GetMovementStrategy(oldMovement, _bankAccountRepository, _historicMovementRepository, _incomeRepository);
            var newMovement = new Movement(savingEditModel);

            strategy.UpdateDebit(newMovement);

            // Update the GenerateIncomeId.
            savingModel.GeneratedIncomeId = newMovement.TargetIncomeId.Value;
            _savingRepository.Update(savingModel);
        }

        public SavingEditModel GetById(int id)
        {
            var saving = _savingRepository.GetList().Include(u => u.TargetInternalAccount).ToList().Single(x => x.Id == id);

            if (saving == null)
            {
                return null;
            }

            var mappedSaving = Mapper.Map<SavingEditModel>(saving);

            return mappedSaving;
        }

        public IList<SavingListModel> GetSavingsByAccountId(int accountId)
        {
            var savings = _savingRepository.GetList()
                 .Include(u => u.Account.Currency)
                 .Include(u => u.TargetInternalAccount)
                 .Where(x => x.Account.Id == accountId)
                 .ToList();

            var mappedSavings = savings.Select(x => Mapper.Map<SavingListModel>(x));

            return mappedSavings.ToList();
        }
    }
}