using PersonalFinanceManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using PersonalFinanceManager.Entities;
using AutoMapper;
using PersonalFinanceManager.Models.Income;
using System.Data.Entity.Validation;
using System.Diagnostics;
using PersonalFinanceManager.DataAccess;
using PersonalFinanceManager.Services.ExpenditureStrategy;
using PersonalFinanceManager.Entities.Enumerations;
using PersonalFinanceManager.Services.Extensions;

namespace PersonalFinanceManager.Services
{
    public class IncomeService: IDisposable
    {
        ApplicationDbContext db;

        public IncomeService()
        {
            db = new ApplicationDbContext();
        }

        public void CreateIncome(IncomeEditModel incomeEditModel)
        {
            var incomeModel = Mapper.Map<IncomeModel>(incomeEditModel);
            db.IncomeModels.Add(incomeModel);
            db.SaveChanges();

            var accountModel = db.AccountModels.SingleOrDefault(x => x.Id == incomeModel.AccountId);
            accountModel.Credit(db, incomeModel.Cost, MovementType.Income);
        }

        public IList<IncomeListModel> GetIncomes(int accountId)
        {
            var incomes = db.IncomeModels.Include(u => u.Account.Currency).Where(x => x.AccountId == accountId).ToList();

            var incomesModel = incomes.Select(x => Mapper.Map<IncomeListModel>(x));
            
            return incomesModel.ToList();
        }

        public IncomeEditModel GetById(int id)
        {
            var income = db.IncomeModels.SingleOrDefault(x => x.Id == id);

            if (income == null)
            {
                return null;
            }

            return Mapper.Map<IncomeEditModel>(income);
        }

        public void EditIncome(IncomeEditModel incomeEditModel)
        {
            var income = db.IncomeModels.Single(x => x.Id == incomeEditModel.Id);

            var oldCost = income.Cost;

            income.Description = incomeEditModel.Description;
            income.Cost = incomeEditModel.Cost;
            income.AccountId = incomeEditModel.AccountId;
            income.DateIncome = incomeEditModel.DateIncome;

            db.Entry(income).State = EntityState.Modified;

            db.SaveChanges();

            if (oldCost != income.Cost)
            {
                var account = db.AccountModels.SingleOrDefault(x => x.Id == income.AccountId);
                account.Credit(db, oldCost, MovementType.Income);
                account.Debit(db, income.Cost, MovementType.Income);
            }
        }

        public void DeleteIncome(int id)
        {
            IncomeModel incomeModel = db.IncomeModels.Find(id);
            db.IncomeModels.Remove(incomeModel);
            db.SaveChanges();

            var accountModel = db.AccountModels.SingleOrDefault(x => x.Id == incomeModel.AccountId);
            accountModel.Debit(db, incomeModel.Cost, MovementType.Income);
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}