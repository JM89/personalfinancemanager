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
        ApplicationDbContext db;

        public AtmWithdrawService()
        {
            db = new ApplicationDbContext();
        }

        public IList<AtmWithdrawListModel> GetAtmWithdrawsByAccountId(int accountId)
        {
            var atmWithdraws = db.AtmWithdrawModels
                .Include(u => u.Account.Currency)
                .Where(x => x.Account.Id == accountId)
                .ToList();

            var expenditures = db.ExpenditureModels;

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

            db.AtmWithdrawModels.Add(atmWithdrawModel);

            db.SaveChanges();

            var accountModel = db.AccountModels.SingleOrDefault(x => x.Id == atmWithdrawModel.AccountId);
            accountModel.CurrentBalance -= atmWithdrawModel.InitialAmount;

            db.Entry(accountModel).State = EntityState.Modified;

            HistoricMovementHelper.SaveDebitMovement(db, atmWithdrawModel.AccountId, atmWithdrawModel.InitialAmount, TargetOptions.Account, MovementType.AtmWithdraw);

            db.SaveChanges();
        }

        public AtmWithdrawEditModel GetById(int id)
        {
            var atmWithdraw = db.AtmWithdrawModels.SingleOrDefault(x => x.Id == id);

            if (atmWithdraw == null)
            {
                return null;
            }

            return Mapper.Map<AtmWithdrawEditModel>(atmWithdraw);
        }

        public void EditAtmWithdraw(AtmWithdrawEditModel atmWithdrawEditModel)
        {
            var atmWithdrawModel = db.AtmWithdrawModels.SingleOrDefault(x => x.Id == atmWithdrawEditModel.Id);

            var oldCost = atmWithdrawModel.InitialAmount;
            atmWithdrawModel.InitialAmount = atmWithdrawEditModel.InitialAmount;
            atmWithdrawModel.CurrentAmount = atmWithdrawEditModel.InitialAmount;
            atmWithdrawModel.DateExpenditure = atmWithdrawEditModel.DateExpenditure;
            atmWithdrawModel.HasBeenAlreadyDebited = atmWithdrawEditModel.HasBeenAlreadyDebited;

            db.Entry(atmWithdrawModel).State = EntityState.Modified;

            db.SaveChanges();

            if (oldCost != atmWithdrawModel.InitialAmount)
            {
                var accountModel = db.AccountModels.SingleOrDefault(x => x.Id == atmWithdrawModel.AccountId);
                accountModel.CurrentBalance += oldCost;
                accountModel.CurrentBalance -= atmWithdrawModel.InitialAmount;

                db.Entry(accountModel).State = EntityState.Modified;

                HistoricMovementHelper.SaveCreditMovement(db, atmWithdrawModel.AccountId, oldCost, TargetOptions.Account, MovementType.AtmWithdraw);
                HistoricMovementHelper.SaveDebitMovement(db, atmWithdrawModel.AccountId, atmWithdrawModel.InitialAmount, TargetOptions.Account, MovementType.AtmWithdraw);
            }

            db.SaveChanges();
        }


        public void CloseAtmWithdraw(int id)
        {
            var atmWithdrawModel = db.AtmWithdrawModels.Single(x => x.Id == id);

            atmWithdrawModel.IsClosed = true;

            db.Entry(atmWithdrawModel).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void DeleteAtmWithdraw(int id)
        {
            AtmWithdrawModel atmWithdrawModel = db.AtmWithdrawModels.Find(id);

            var accountModel = db.AccountModels.SingleOrDefault(x => x.Id == atmWithdrawModel.AccountId);
            accountModel.CurrentBalance += atmWithdrawModel.InitialAmount;
            db.Entry(accountModel).State = EntityState.Modified;

            HistoricMovementHelper.SaveCreditMovement(db, atmWithdrawModel.AccountId, atmWithdrawModel.InitialAmount, TargetOptions.Account, MovementType.AtmWithdraw);

            db.AtmWithdrawModels.Remove(atmWithdrawModel);

            db.SaveChanges();
        }

        public void ChangeDebitStatus(int id, bool debitStatus)
        {
            AtmWithdrawModel atmWithdrawModel = db.AtmWithdrawModels.SingleOrDefault(x => x.Id == id);
            atmWithdrawModel.HasBeenAlreadyDebited = debitStatus;
            db.Entry(atmWithdrawModel).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}