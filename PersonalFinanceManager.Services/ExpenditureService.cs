﻿using PersonalFinanceManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using PersonalFinanceManager.Entities;
using AutoMapper;
using PersonalFinanceManager.Models.Expenditure;
using PersonalFinanceManager.DataAccess;
using PersonalFinanceManager.Entities.Enumerations;
using PersonalFinanceManager.Services.ExpenditureStrategy;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.Services.RequestObjects;

namespace PersonalFinanceManager.Services
{
    public class ExpenditureService : IExpenditureService
    {
        ApplicationDbContext db;

        public ExpenditureService()
        {
            db = new ApplicationDbContext();
        }

        public void CreateExpenditure(ExpenditureEditModel expenditureEditModel)
        {
            var expenditureModel = Mapper.Map<ExpenditureModel>(expenditureEditModel);

            db.ExpenditureModels.Add(expenditureModel);

            db.SaveChanges();

            var strategy = ContextExpenditureStrategy.GetExpenditureStrategy(db, expenditureModel);

            strategy.Debit();
        }

        
        public void EditExpenditure(ExpenditureEditModel expenditureEditModel)
        {
            var expenditureModel = db.ExpenditureModels.Single(x => x.Id == expenditureEditModel.Id);

            var oldExpenditureModel = new ExpenditureModel() {
                AccountId = expenditureModel.AccountId, 
                Cost = expenditureModel.Cost,
                AtmWithdrawId = expenditureModel.AtmWithdrawId,
                DateExpenditure = expenditureModel.DateExpenditure,
                Description = expenditureModel.Description, 
                PaymentMethodId = expenditureModel.PaymentMethodId,
                TargetInternalAccountId = expenditureModel.TargetInternalAccountId
            };
            
            var strategy = ContextExpenditureStrategy.GetExpenditureStrategy(db, oldExpenditureModel);

            expenditureModel.DateExpenditure = expenditureEditModel.DateExpenditure;
            expenditureModel.Description = expenditureEditModel.Description;
            expenditureModel.AccountId = expenditureEditModel.AccountId;
            expenditureModel.PaymentMethodId = expenditureEditModel.PaymentMethodId;
            expenditureModel.TypeExpenditureId = expenditureEditModel.TypeExpenditureId;
            expenditureModel.Cost = expenditureEditModel.Cost;
            expenditureModel.HasBeenAlreadyDebited = expenditureEditModel.HasBeenAlreadyDebited;
            expenditureModel.AtmWithdrawId = expenditureEditModel.AtmWithdrawId;
            expenditureModel.TargetInternalAccountId = expenditureEditModel.TargetInternalAccountId;

            db.Entry(expenditureModel).State = EntityState.Modified;

            db.SaveChanges();
            
            strategy.UpdateDebit(expenditureModel);
        }

        public void DeleteExpenditure(int id)
        {
            ExpenditureModel expenditureModel = db.ExpenditureModels.Find(id);

            var strategy = ContextExpenditureStrategy.GetExpenditureStrategy(db, expenditureModel);

            strategy.Credit();

            db.ExpenditureModels.Remove(expenditureModel);
            db.SaveChanges();
        }

        public ExpenditureEditModel GetById(int id)
        {
            var expenditure = db.ExpenditureModels
                                    .Include(u => u.Account.Currency)
                                    .Include(u => u.TypeExpenditure)
                                    .Include(u => u.PaymentMethod).SingleOrDefault(x => x.Id == id);

            if (expenditure == null)
            {
                return null;
            }

            return Mapper.Map<ExpenditureEditModel>(expenditure);
        }

        public void ChangeDebitStatus(int id, bool debitStatus)
        {
            var expenditure = db.ExpenditureModels.Single(x => x.Id == id);
            expenditure.HasBeenAlreadyDebited = debitStatus;
            db.Entry(expenditure).State = EntityState.Modified;
            db.SaveChanges();
        }

        public IList<ExpenditureListModel> GetExpenditures(ExpenditureSearch search)
        {
            var expenditures = db.ExpenditureModels
                .Where(x =>
                    (!search.AccountId.HasValue || (search.AccountId.HasValue && x.AccountId == search.AccountId.Value))
                    && (!search.StartDate.HasValue || (search.StartDate.HasValue && x.DateExpenditure >= search.StartDate))
                    && (!search.EndDate.HasValue || (search.EndDate.HasValue && x.DateExpenditure < search.EndDate))
                    && (!search.ExpenditureTypeId.HasValue || (search.ExpenditureTypeId.HasValue && x.TypeExpenditureId == search.ExpenditureTypeId.Value)))
                .Include(u => u.Account.Currency)
                .Include(u => u.TypeExpenditure)
                .Include(u => u.PaymentMethod).ToList();

            var mappedExpenditures = expenditures.Select(x => Mapper.Map<ExpenditureListModel>(x));

            return mappedExpenditures.ToList();
        }
    }
}