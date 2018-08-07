using System.Collections.Generic;
using System.Linq;
using PFM.Services.Interfaces;
using PFM.Services.Helpers;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DTOs.AtmWithdraw;
using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Enumerations;
using AutoMapper;

namespace PFM.Services
{
    public class AtmWithdrawService : IAtmWithdrawService
    {
        private readonly IAtmWithdrawRepository _atmWithdrawRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IExpenseRepository _expenditureRepository;
        private readonly IHistoricMovementRepository _historicMovementRepository;

        public AtmWithdrawService(IAtmWithdrawRepository atmWithdrawRepository, IBankAccountRepository bankAccountRepository, IExpenseRepository expenditureRepository,
            IHistoricMovementRepository historicMovementRepository)
        {
            this._atmWithdrawRepository = atmWithdrawRepository;
            this._bankAccountRepository = bankAccountRepository;
            this._expenditureRepository = expenditureRepository;
            this._historicMovementRepository = historicMovementRepository;
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

        public void CreateAtmWithdraws(List<AtmWithdrawDetails> atmWithdrawDetails)
        {
            atmWithdrawDetails.ForEach(CreateAtmWithdraw);
        }

        public void CreateAtmWithdraw(AtmWithdrawDetails atmWithdrawDetails)
        {
            var atmWithdraw = Mapper.Map<AtmWithdraw>(atmWithdrawDetails);
            atmWithdraw.CurrentAmount = atmWithdrawDetails.InitialAmount;
            atmWithdraw.IsClosed = false;
            _atmWithdrawRepository.Create(atmWithdraw);

            var account = _bankAccountRepository.GetById(atmWithdraw.AccountId);
            MovementHelpers.Debit(_historicMovementRepository, atmWithdraw.InitialAmount, account.Id, ObjectType.Account, account.CurrentBalance);

            account.CurrentBalance -= atmWithdraw.InitialAmount;
            _bankAccountRepository.Update(account);
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

        public void EditAtmWithdraw(AtmWithdrawDetails atmWithdrawDetails)
        {
            var atmWithdraw = _atmWithdrawRepository.GetById(atmWithdrawDetails.Id);

            var oldCost = atmWithdraw.InitialAmount;

            atmWithdraw.InitialAmount = atmWithdrawDetails.InitialAmount;
            atmWithdraw.CurrentAmount = atmWithdrawDetails.InitialAmount;
            atmWithdraw.DateExpense = atmWithdrawDetails.DateExpense;
            atmWithdraw.HasBeenAlreadyDebited = atmWithdrawDetails.HasBeenAlreadyDebited;

            _atmWithdrawRepository.Update(atmWithdraw);
            
            if (oldCost != atmWithdraw.InitialAmount)
            {
                var account = _bankAccountRepository.GetById(atmWithdraw.AccountId);
                MovementHelpers.Credit(_historicMovementRepository, oldCost, account.Id, ObjectType.Account, account.CurrentBalance);
                account.CurrentBalance += oldCost;
                MovementHelpers.Debit(_historicMovementRepository, atmWithdraw.InitialAmount, account.Id, ObjectType.Account, account.CurrentBalance);
                account.CurrentBalance -= atmWithdraw.InitialAmount;
                _bankAccountRepository.Update(account);
            }
        }
        
        public void CloseAtmWithdraw(int id)
        {
            var atmWithdraw = _atmWithdrawRepository.GetById(id); 
            atmWithdraw.IsClosed = true;
            _atmWithdrawRepository.Update(atmWithdraw);
        }

        public void DeleteAtmWithdraw(int id)
        {
            var atmWithdraw = _atmWithdrawRepository.GetById(id);

            var account = _bankAccountRepository.GetById(atmWithdraw.AccountId);
            MovementHelpers.Debit(_historicMovementRepository, atmWithdraw.InitialAmount, account.Id, ObjectType.Account, account.CurrentBalance);
            account.CurrentBalance += atmWithdraw.InitialAmount;
            _bankAccountRepository.Update(account);

            _atmWithdrawRepository.Delete(atmWithdraw);
        }

        public void ChangeDebitStatus(int id, bool debitStatus)
        {
            AtmWithdraw atmWithdraw = _atmWithdrawRepository.GetById(id);
            atmWithdraw.HasBeenAlreadyDebited = debitStatus;
            _atmWithdrawRepository.Update(atmWithdraw);
        }
    }
}