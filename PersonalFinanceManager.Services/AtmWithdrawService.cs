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

namespace PersonalFinanceManager.Services
{
    public class AtmWithdrawService : IAtmWithdrawService
    {
        private ApplicationDbContext _db;

        public AtmWithdrawService(ApplicationDbContext db)
        {
            this._db = db;
        }
        
        public IList<AtmWithdrawListModel> GetAtmWithdrawsByAccountId(int accountId)
        {
            var atmWithdraws = _db.AtmWithdrawModels
                .Include(u => u.Account.Currency)
                .Where(x => x.Account.Id == accountId)
                .ToList();

            var expenditures = _db.ExpenditureModels;

            var mappedAtmWithdraws = atmWithdraws.Select(x => Mapper.Map<AtmWithdrawListModel>(x)).ToList();

            mappedAtmWithdraws.ForEach(atmWithdraw =>
            {
                atmWithdraw.CanBeDeleted = !expenditures.Any(x => x.AtmWithdrawId == atmWithdraw.Id);
                atmWithdraw.CanBeEdited = !expenditures.Any(x => x.AtmWithdrawId == atmWithdraw.Id); ;
            });

            return mappedAtmWithdraws;
        }

        public void CreateAtmWithdraw(AtmWithdrawEditModel atmWithdrawEditModel)
        {
            var atmWithdrawModel = Mapper.Map<AtmWithdrawModel>(atmWithdrawEditModel);

            atmWithdrawModel.CurrentAmount = atmWithdrawEditModel.InitialAmount;
            atmWithdrawModel.IsClosed = false;

            _db.AtmWithdrawModels.Add(atmWithdrawModel);

            _db.SaveChanges();

            var accountModel = _db.AccountModels.SingleOrDefault(x => x.Id == atmWithdrawModel.AccountId);
            accountModel.CurrentBalance -= atmWithdrawModel.InitialAmount;

            _db.Entry(accountModel).State = EntityState.Modified;

            HistoricMovementHelper.SaveDebitMovement(_db, atmWithdrawModel.AccountId, atmWithdrawModel.InitialAmount, TargetOptions.Account, MovementType.AtmWithdraw);

            _db.SaveChanges();
        }

        public AtmWithdrawEditModel GetById(int id)
        {
            var atmWithdraw = _db.AtmWithdrawModels.SingleOrDefault(x => x.Id == id);

            if (atmWithdraw == null)
            {
                return null;
            }

            return Mapper.Map<AtmWithdrawEditModel>(atmWithdraw);
        }

        public void EditAtmWithdraw(AtmWithdrawEditModel atmWithdrawEditModel)
        {
            var atmWithdrawModel = _db.AtmWithdrawModels.SingleOrDefault(x => x.Id == atmWithdrawEditModel.Id);

            var oldCost = atmWithdrawModel.InitialAmount;
            atmWithdrawModel.InitialAmount = atmWithdrawEditModel.InitialAmount;
            atmWithdrawModel.CurrentAmount = atmWithdrawEditModel.InitialAmount;
            atmWithdrawModel.DateExpenditure = atmWithdrawEditModel.DateExpenditure;
            atmWithdrawModel.HasBeenAlreadyDebited = atmWithdrawEditModel.HasBeenAlreadyDebited;

            _db.Entry(atmWithdrawModel).State = EntityState.Modified;

            _db.SaveChanges();

            if (oldCost != atmWithdrawModel.InitialAmount)
            {
                var accountModel = _db.AccountModels.SingleOrDefault(x => x.Id == atmWithdrawModel.AccountId);
                accountModel.CurrentBalance += oldCost;
                accountModel.CurrentBalance -= atmWithdrawModel.InitialAmount;

                _db.Entry(accountModel).State = EntityState.Modified;

                HistoricMovementHelper.SaveCreditMovement(_db, atmWithdrawModel.AccountId, oldCost, TargetOptions.Account, MovementType.AtmWithdraw);
                HistoricMovementHelper.SaveDebitMovement(_db, atmWithdrawModel.AccountId, atmWithdrawModel.InitialAmount, TargetOptions.Account, MovementType.AtmWithdraw);
            }

            _db.SaveChanges();
        }


        public void CloseAtmWithdraw(int id)
        {
            var atmWithdrawModel = _db.AtmWithdrawModels.Single(x => x.Id == id);

            atmWithdrawModel.IsClosed = true;

            _db.Entry(atmWithdrawModel).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void DeleteAtmWithdraw(int id)
        {
            AtmWithdrawModel atmWithdrawModel = _db.AtmWithdrawModels.Find(id);

            var accountModel = _db.AccountModels.SingleOrDefault(x => x.Id == atmWithdrawModel.AccountId);
            accountModel.CurrentBalance += atmWithdrawModel.InitialAmount;
            _db.Entry(accountModel).State = EntityState.Modified;

            HistoricMovementHelper.SaveCreditMovement(_db, atmWithdrawModel.AccountId, atmWithdrawModel.InitialAmount, TargetOptions.Account, MovementType.AtmWithdraw);

            _db.AtmWithdrawModels.Remove(atmWithdrawModel);

            _db.SaveChanges();
        }

        public void ChangeDebitStatus(int id, bool debitStatus)
        {
            AtmWithdrawModel atmWithdrawModel = _db.AtmWithdrawModels.SingleOrDefault(x => x.Id == id);
            atmWithdrawModel.HasBeenAlreadyDebited = debitStatus;
            _db.Entry(atmWithdrawModel).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}