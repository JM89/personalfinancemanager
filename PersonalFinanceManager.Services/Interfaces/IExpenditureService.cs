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

namespace PersonalFinanceManager.Services
{
    public interface IExpenditureService : IBaseService
    {
        [Obsolete]
        IList<ExpenditureModel> GetExpendituresByAccountId(int accountId);

        IList<ExpenditureModel> GetExpendituresByAccountIdForDashboard(int accountId, DateTime startDate, DateTime endDate);

        IList<ExpenditureListModel> GetExpendituresByAccountId2(int accountId);

        IList<ExpenditureListModel> GetAll();

        void CreateExpenditure(ExpenditureEditModel expenditureEditModel);

        void EditExpenditure(ExpenditureEditModel expenditureEditModel);

        void DeleteExpenditure(int id);

        ExpenditureEditModel GetById(int id);

        void ChangeDebitStatus(int id, bool debitStatus);
    }
}