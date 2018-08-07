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
            CreateMap<DTOs.Expense.ExpenseDetails, DataAccessLayer.Entities.Expense>();
            CreateMap<DTOs.Expense.ExpenseList, DataAccessLayer.Entities.Expense>();

            CreateMap<DTOs.Bank.BankDetails, DataAccessLayer.Entities.Bank>();
            CreateMap<DTOs.Bank.BankBranchDetails, DataAccessLayer.Entities.BankBranch>();

            CreateMap<DTOs.AtmWithdraw.AtmWithdrawList, DataAccessLayer.Entities.AtmWithdraw>();
            CreateMap<DTOs.AtmWithdraw.AtmWithdrawDetails, DataAccessLayer.Entities.AtmWithdraw>();

            CreateMap<DTOs.PaymentMethod.PaymentMethodList, DataAccessLayer.Entities.PaymentMethod>();

            CreateMap<DTOs.Country.CountryList, DataAccessLayer.Entities.Country>();
            CreateMap<DTOs.Country.CountryDetails, DataAccessLayer.Entities.Country>();

            CreateMap<DTOs.Income.IncomeList, DataAccessLayer.Entities.Income>();
            CreateMap<DTOs.Income.IncomeDetails, DataAccessLayer.Entities.Income>();

            CreateMap<DTOs.ExpenseType.ExpenseTypeList, DataAccessLayer.Entities.ExpenseType>();
            CreateMap<DTOs.ExpenseType.ExpenseTypeDetails, DataAccessLayer.Entities.ExpenseType>();

            CreateMap<DTOs.Currency.CurrencyList, DataAccessLayer.Entities.Currency>();
            CreateMap<DTOs.Currency.CurrencyDetails, DataAccessLayer.Entities.Currency>();

            CreateMap<DTOs.Account.AccountList, DataAccessLayer.Entities.Account>();
            CreateMap<DTOs.Account.AccountDetails, DataAccessLayer.Entities.Account>();

            CreateMap<DTOs.BudgetPlan.BudgetPlanList, DataAccessLayer.Entities.BudgetPlan>();
            CreateMap<DTOs.BudgetPlan.BudgetPlanDetails, DataAccessLayer.Entities.BudgetPlan>();

            CreateMap<DTOs.UserProfile.UserProfileDetails, DataAccessLayer.Entities.UserProfile>();

            CreateMap<DTOs.Saving.SavingDetails, DataAccessLayer.Entities.Saving>();

            CreateMap<DTOs.Pension.PensionDetails, DataAccessLayer.Entities.Pension>();

            CreateMap<DTOs.Tax.TaxDetails, DataAccessLayer.Entities.Tax>();

            CreateMap<DTOs.Salary.SalaryDetails, DataAccessLayer.Entities.Salary>();
            CreateMap<DTOs.Salary.SalaryDeductionDetails, DataAccessLayer.Entities.SalaryDeduction>();
        }
    }
}
