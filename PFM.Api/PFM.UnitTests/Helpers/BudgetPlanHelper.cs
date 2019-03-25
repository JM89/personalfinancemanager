using PFM.Services.DTOs.BudgetPlan;
using PFM.Services.DTOs.ExpenseType;
using System.Collections.Generic;

namespace PFM.UnitTests.Helpers
{
    public static class BudgetPlanHelper
    {
        public static BudgetPlanDetails CreateBudgetPlanEditModel(List<ExpenseTypeList> expenditureTypes)
        {
            var model = new BudgetPlanDetails()
            {
                ExpenseTypes = new List<BudgetPlanExpenseType>()
             };
            foreach (var expenditureType in expenditureTypes)
            {
                model.ExpenseTypes.Add(new BudgetPlanExpenseType()
                {
                    ExpenseType = expenditureType, 
                    ExpectedValue = 100
                });
            }

            return model;
        }
    }
}
