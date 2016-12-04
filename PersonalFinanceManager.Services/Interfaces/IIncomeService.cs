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
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IIncomeService : IBaseService
    {
        void CreateIncome(IncomeEditModel incomeEditModel);

        IList<IncomeListModel> GetIncomes(int accountId);

        IncomeEditModel GetById(int id);

        void EditIncome(IncomeEditModel incomeEditModel);

        void DeleteIncome(int id);
    }
}