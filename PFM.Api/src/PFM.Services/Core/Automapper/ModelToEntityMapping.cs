using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Services.Core.Automapper
{
    public class ModelToEntityMapping : Profile
    {
        public ModelToEntityMapping()
        {
            CreateMap<Api.Contracts.Expense.ExpenseDetails, DataAccessLayer.Entities.Expense>();
            CreateMap<Api.Contracts.Expense.ExpenseList, DataAccessLayer.Entities.Expense>();

            CreateMap<Api.Contracts.AtmWithdraw.AtmWithdrawList, DataAccessLayer.Entities.AtmWithdraw>();
            CreateMap<Api.Contracts.AtmWithdraw.AtmWithdrawDetails, DataAccessLayer.Entities.AtmWithdraw>();

            CreateMap<Api.Contracts.PaymentMethod.PaymentMethodList, DataAccessLayer.Entities.PaymentMethod>();

            CreateMap<Api.Contracts.Income.IncomeList, DataAccessLayer.Entities.Income>();
            CreateMap<Api.Contracts.Income.IncomeDetails, DataAccessLayer.Entities.Income>();

            CreateMap<Api.Contracts.ExpenseType.ExpenseTypeList, DataAccessLayer.Entities.ExpenseType>();
            CreateMap<Api.Contracts.ExpenseType.ExpenseTypeDetails, DataAccessLayer.Entities.ExpenseType>();

            CreateMap<Api.Contracts.BudgetPlan.BudgetPlanList, DataAccessLayer.Entities.BudgetPlan>();
            CreateMap<Api.Contracts.BudgetPlan.BudgetPlanDetails, DataAccessLayer.Entities.BudgetPlan>();

            CreateMap<Api.Contracts.UserProfile.UserProfileDetails, DataAccessLayer.Entities.UserProfile>();

            CreateMap<Api.Contracts.Saving.SavingDetails, DataAccessLayer.Entities.Saving>();

            CreateMap<Api.Contracts.Pension.PensionDetails, DataAccessLayer.Entities.Pension>();

            CreateMap<Api.Contracts.Tax.TaxDetails, DataAccessLayer.Entities.Tax>();

            CreateMap<Api.Contracts.Salary.SalaryDetails, DataAccessLayer.Entities.Salary>();
            CreateMap<Api.Contracts.Salary.SalaryDeductionDetails, DataAccessLayer.Entities.SalaryDeduction>();
        }
    }
}
