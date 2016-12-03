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
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Services
{
    public class IncomeService: IDisposable, IIncomeService
    {
        private ApplicationDbContext _db;

        public IncomeService(ApplicationDbContext db)
        {
            this._db = db;
        }

        public void CreateIncome(IncomeEditModel incomeEditModel)
        {
            var incomeModel = Mapper.Map<IncomeModel>(incomeEditModel);
            _db.IncomeModels.Add(incomeModel);
            _db.SaveChanges();

            var accountModel = _db.AccountModels.SingleOrDefault(x => x.Id == incomeModel.AccountId);
            accountModel.Credit(_db, incomeModel.Cost, MovementType.Income);
        }

        public IList<IncomeListModel> GetIncomes(int accountId)
        {
            var incomes = _db.IncomeModels.Include(u => u.Account.Currency).Where(x => x.AccountId == accountId).ToList();

            var incomesModel = incomes.Select(x => Mapper.Map<IncomeListModel>(x));
            
            return incomesModel.ToList();
        }

        public IncomeEditModel GetById(int id)
        {
            var income = _db.IncomeModels.SingleOrDefault(x => x.Id == id);

            if (income == null)
            {
                return null;
            }

            return Mapper.Map<IncomeEditModel>(income);
        }

        public void EditIncome(IncomeEditModel incomeEditModel)
        {
            var income = _db.IncomeModels.Single(x => x.Id == incomeEditModel.Id);

            var oldCost = income.Cost;

            income.Description = incomeEditModel.Description;
            income.Cost = incomeEditModel.Cost;
            income.AccountId = incomeEditModel.AccountId;
            income.DateIncome = incomeEditModel.DateIncome;

            _db.Entry(income).State = EntityState.Modified;

            _db.SaveChanges();

            if (oldCost != income.Cost)
            {
                var account = _db.AccountModels.SingleOrDefault(x => x.Id == income.AccountId);
                account.Credit(_db, oldCost, MovementType.Income);
                account.Debit(_db, income.Cost, MovementType.Income);
            }
        }

        public void DeleteIncome(int id)
        {
            IncomeModel incomeModel = _db.IncomeModels.Find(id);
            _db.IncomeModels.Remove(incomeModel);
            _db.SaveChanges();

            var accountModel = _db.AccountModels.SingleOrDefault(x => x.Id == incomeModel.AccountId);
            accountModel.Debit(_db, incomeModel.Cost, MovementType.Income);
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}