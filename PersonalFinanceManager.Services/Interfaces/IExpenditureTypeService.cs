using PersonalFinanceManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Models.ExpenditureType;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using PersonalFinanceManager.DataAccess;
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IExpenditureTypeService : IBaseService
    {
        IList<ExpenditureTypeListModel> GetExpenditureTypes();

        ExpenditureTypeEditModel GetById(int id);

        void CreateExpenditureType(ExpenditureTypeEditModel expenditureTypeEditModel);

        void EditExpenditureType(ExpenditureTypeEditModel expenditureTypeEditModel);

        void DeleteExpenditureType(int id);
    }
}