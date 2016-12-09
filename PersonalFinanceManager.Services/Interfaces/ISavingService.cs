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
using PersonalFinanceManager.Models.Saving;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface ISavingService : IBaseService
    {
        IList<SavingListModel> GetSavingsByAccountId(int accountId);

        void CreateSaving(SavingEditModel savingEditModel);

        SavingEditModel GetById(int id);

        void EditSaving(SavingEditModel savingEditModel);

        void DeleteSaving(int id);
    }
}