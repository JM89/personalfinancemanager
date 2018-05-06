using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using PersonalFinanceManager.Entities;
using AutoMapper;
using PersonalFinanceManager.Models.AtmWithdraw;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Services.Helpers;
using PersonalFinanceManager.Entities.Enumerations;

namespace PersonalFinanceManager.Services
{
    public class AtmWithdrawService : IAtmWithdrawService
    {
        private readonly IAtmWithdrawRepository _atmWithdrawRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IExpenditureRepository _expenditureRepository;
        private readonly IHistoricMovementRepository _historicMovementRepository;

        public AtmWithdrawService(IAtmWithdrawRepository atmWithdrawRepository, IBankAccountRepository bankAccountRepository, IExpenditureRepository expenditureRepository,
            IHistoricMovementRepository historicMovementRepository)
        {
            this._atmWithdrawRepository = atmWithdrawRepository;
            this._bankAccountRepository = bankAccountRepository;
            this._expenditureRepository = expenditureRepository;
            this._historicMovementRepository = historicMovementRepository;
        }
              
        public IList<AtmWithdrawListModel> GetAtmWithdrawsByAccountId(int accountId)
        {
            var atmWithdraws = _atmWithdrawRepository.GetList()
                .Include(u => u.Account.Currency)
                .Where(x => x.Account.Id == accountId)
                .ToList();

            var expenditures = _expenditureRepository.GetList();

            var mappedAtmWithdraws = atmWithdraws.Select(Mapper.Map<AtmWithdrawListModel>).ToList();

            mappedAtmWithdraws.ForEach(atmWithdraw =>
            {
                atmWithdraw.CanBeDeleted = !expenditures.Any(x => x.AtmWithdrawId == atmWithdraw.Id);
                atmWithdraw.CanBeEdited = !expenditures.Any(x => x.AtmWithdrawId == atmWithdraw.Id); 
            });

            return mappedAtmWithdraws;
        }

        public void CreateAtmWithdraws(List<AtmWithdrawEditModel> atmWithdrawEditModel)
        {
            atmWithdrawEditModel.ForEach(CreateAtmWithdraw);
        }

        public void CreateAtmWithdraw(AtmWithdrawEditModel atmWithdrawEditModel)
        {
            var atmWithdrawModel = Mapper.Map<AtmWithdrawModel>(atmWithdrawEditModel);
            atmWithdrawModel.CurrentAmount = atmWithdrawEditModel.InitialAmount;
            atmWithdrawModel.IsClosed = false;
            _atmWithdrawRepository.Create(atmWithdrawModel);

            var account = _bankAccountRepository.GetById(atmWithdrawModel.AccountId);
            MovementHelpers.Debit(_historicMovementRepository, atmWithdrawModel.InitialAmount, account.Id, ObjectType.Account, account.CurrentBalance);

            account.CurrentBalance -= atmWithdrawModel.InitialAmount;
            _bankAccountRepository.Update(account);
        }

        public AtmWithdrawEditModel GetById(int id)
        {
            var atmWithdraw = _atmWithdrawRepository.GetById(id);

            if (atmWithdraw == null)
            {
                return null;
            }

            return Mapper.Map<AtmWithdrawEditModel>(atmWithdraw);
        }

        public void EditAtmWithdraw(AtmWithdrawEditModel atmWithdrawEditModel)
        {
            var atmWithdrawModel = _atmWithdrawRepository.GetById(atmWithdrawEditModel.Id);

            var oldCost = atmWithdrawModel.InitialAmount;

            atmWithdrawModel.InitialAmount = atmWithdrawEditModel.InitialAmount;
            atmWithdrawModel.CurrentAmount = atmWithdrawEditModel.InitialAmount;
            atmWithdrawModel.DateExpenditure = atmWithdrawEditModel.DateExpenditure;
            atmWithdrawModel.HasBeenAlreadyDebited = atmWithdrawEditModel.HasBeenAlreadyDebited;

            _atmWithdrawRepository.Update(atmWithdrawModel);
            
            if (oldCost != atmWithdrawModel.InitialAmount)
            {
                var account = _bankAccountRepository.GetById(atmWithdrawModel.AccountId);
                MovementHelpers.Credit(_historicMovementRepository, oldCost, account.Id, ObjectType.Account, account.CurrentBalance);
                account.CurrentBalance += oldCost;
                MovementHelpers.Debit(_historicMovementRepository, atmWithdrawModel.InitialAmount, account.Id, ObjectType.Account, account.CurrentBalance);
                account.CurrentBalance -= atmWithdrawModel.InitialAmount;
                _bankAccountRepository.Update(account);
            }
        }
        
        public void CloseAtmWithdraw(int id)
        {
            var atmWithdrawModel = _atmWithdrawRepository.GetById(id); 
            atmWithdrawModel.IsClosed = true;
            _atmWithdrawRepository.Update(atmWithdrawModel);
        }

        public void DeleteAtmWithdraw(int id)
        {
            var atmWithdrawModel = _atmWithdrawRepository.GetById(id);

            var account = _bankAccountRepository.GetById(atmWithdrawModel.AccountId);
            MovementHelpers.Debit(_historicMovementRepository, atmWithdrawModel.InitialAmount, account.Id, ObjectType.Account, account.CurrentBalance);
            account.CurrentBalance += atmWithdrawModel.InitialAmount;
            _bankAccountRepository.Update(account);

            _atmWithdrawRepository.Delete(atmWithdrawModel);
        }

        public void ChangeDebitStatus(int id, bool debitStatus)
        {
            AtmWithdrawModel atmWithdrawModel = _atmWithdrawRepository.GetById(id);
            atmWithdrawModel.HasBeenAlreadyDebited = debitStatus;
            _atmWithdrawRepository.Update(atmWithdrawModel);
        }
    }
}