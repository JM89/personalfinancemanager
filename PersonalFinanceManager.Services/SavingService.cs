using PersonalFinanceManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PersonalFinanceManager.Entities;
using AutoMapper;
using PersonalFinanceManager.Models.Expenditure;
using System.Data.Entity.Validation;
using PersonalFinanceManager.DataAccess;
using PersonalFinanceManager.Models.AtmWithdraw;
using System.Diagnostics;
using System.Data.Entity.Infrastructure;
using PersonalFinanceManager.Services.ExpenditureStrategy;
using PersonalFinanceManager.Entities.Enumerations;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Models.Saving;
using PersonalFinanceManager.Services.Helpers;

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
            _savingRepository.Create(savingModel);
        }

        public void DeleteSaving(int id)
        {
            var savingModel = _savingRepository.GetById(id);
            _savingRepository.Delete(savingModel);
        }

        public void EditSaving(SavingEditModel savingEditModel)
        {
            var savingModel = _savingRepository.GetList().AsNoTracking().SingleOrDefault(x => x.Id == savingEditModel.Id);
            savingModel = Mapper.Map<SavingModel>(savingEditModel);
            _savingRepository.Update(savingModel);
        }

        public SavingEditModel GetById(int id)
        {
            var saving = _savingRepository.GetById(id);

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
                 .Where(x => x.Account.Id == accountId)
                 .ToList();

            var mappedSavings = savings.Select(x => Mapper.Map<SavingListModel>(x));

            return mappedSavings.ToList();
        }
    }
}