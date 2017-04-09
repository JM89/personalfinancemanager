using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Models.BudgetPlan;
using PersonalFinanceManager.Models.ExpenditureType;

namespace PersonalFinanceManager.Tests.Helpers
{
    public static class BudgetPlanHelper
    {
        public static BudgetPlanEditModel CreateBudgetPlanEditModel(List<ExpenditureTypeListModel> expenditureTypes)
        {
            var model = new BudgetPlanEditModel()
            {
                ExpenditureTypes = new List<BudgetPlanExpenditureType>()
             };
            foreach (var expenditureType in expenditureTypes)
            {
                model.ExpenditureTypes.Add(new BudgetPlanExpenditureType()
                {
                    ExpenditureType = expenditureType, 
                    ExpectedValue = 100
                });
            }

            return model;
        }
    }
}
