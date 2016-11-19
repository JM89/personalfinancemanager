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
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IAtmWithdrawService : IBaseService
    {
        IList<AtmWithdrawListModel> GetAtmWithdrawsByAccountId(int accountId);

        void CreateAtmWithdraw(AtmWithdrawEditModel atmWithdrawEditModel);

        AtmWithdrawEditModel GetById(int id);

        void EditAtmWithdraw(AtmWithdrawEditModel atmWithdrawEditModel);

        void CloseAtmWithdraw(int id);

        void DeleteAtmWithdraw(int id);

        void ChangeDebitStatus(int id, bool debitStatus);
    }
}