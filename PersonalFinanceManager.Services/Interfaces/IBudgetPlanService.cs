using PersonalFinanceManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Models.BudgetPlan;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using PersonalFinanceManager.DataAccess;
using PersonalFinanceManager.Models.ExpenditureType;
using PersonalFinanceManager.Models.Dashboard;
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IBudgetPlanService : IBaseService
    {
        IList<BudgetPlanListModel> GetBudgetPlans(int accountId);

        BudgetPlanEditModel GetCurrent(int accountId);

        BudgetPlanEditModel GetById(int id);

        void CreateBudgetPlan(BudgetPlanEditModel budgetPlanEditModel, int accountId);

        void EditBudgetPlan(BudgetPlanEditModel budgetPlanEditModel, int accountId);

        void StartBudgetPlan(int value);

        void StopBudgetPlan(int value);
    }
}