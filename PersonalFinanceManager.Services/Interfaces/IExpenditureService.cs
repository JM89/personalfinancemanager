using PersonalFinanceManager.Models;
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
using PersonalFinanceManager.Services.Core;
using PersonalFinanceManager.Services.RequestObjects;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IExpenditureService : IBaseService
    {
        void CreateExpenditure(ExpenditureEditModel expenditureEditModel);

        void EditExpenditure(ExpenditureEditModel expenditureEditModel);

        void DeleteExpenditure(int id);

        ExpenditureEditModel GetById(int id);

        void ChangeDebitStatus(int id, bool debitStatus);

        IList<ExpenditureListModel> GetExpenditures(ExpenditureSearch search);
    }
}