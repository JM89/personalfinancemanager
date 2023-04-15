using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Services.Core.Automapper
{
    public class EntityToModelMapping : Profile
    {
        public EntityToModelMapping()
        {
            CreateMap<DataAccessLayer.Entities.Expense, Api.Contracts.Expense.ExpenseList>();
            CreateMap<DataAccessLayer.Entities.Expense, Api.Contracts.Expense.ExpenseDetails>();
            
            CreateMap<DataAccessLayer.Entities.PaymentMethod, Api.Contracts.PaymentMethod.PaymentMethodList>();

            CreateMap<DataAccessLayer.Entities.AtmWithdraw, Api.Contracts.AtmWithdraw.AtmWithdrawDetails>();
            CreateMap<DataAccessLayer.Entities.AtmWithdraw, Api.Contracts.AtmWithdraw.AtmWithdrawList>();

            CreateMap<DataAccessLayer.Entities.ExpenseType, Api.Contracts.ExpenseType.ExpenseTypeList>();
            CreateMap<DataAccessLayer.Entities.ExpenseType, Api.Contracts.ExpenseType.ExpenseTypeDetails>();

            CreateMap<DataAccessLayer.Entities.Country, Api.Contracts.Country.CountryList>();
            CreateMap<DataAccessLayer.Entities.Country, Api.Contracts.Country.CountryDetails>();

            CreateMap<DataAccessLayer.Entities.Income, Api.Contracts.Income.IncomeList>();
            CreateMap<DataAccessLayer.Entities.Income, Api.Contracts.Income.IncomeDetails>();

            CreateMap<DataAccessLayer.Entities.Bank, Api.Contracts.Bank.BankList>();
            CreateMap<DataAccessLayer.Entities.Bank, Api.Contracts.Bank.BankDetails>();
            CreateMap<DataAccessLayer.Entities.BankBranch, Api.Contracts.Bank.BankBranchDetails>();

            CreateMap<DataAccessLayer.Entities.Currency, Api.Contracts.Currency.CurrencyList>();
            CreateMap<DataAccessLayer.Entities.Currency, Api.Contracts.Currency.CurrencyDetails>();

            CreateMap<DataAccessLayer.Entities.Account, Api.Contracts.Account.AccountList>();
            CreateMap<DataAccessLayer.Entities.Account, Api.Contracts.Account.AccountDetails>();

            CreateMap<DataAccessLayer.Entities.BudgetPlan, Api.Contracts.BudgetPlan.BudgetPlanList>();
            CreateMap<DataAccessLayer.Entities.BudgetPlan, Api.Contracts.BudgetPlan.BudgetPlanDetails>();

            CreateMap<DataAccessLayer.Entities.UserProfile, Api.Contracts.UserProfile.UserProfileDetails>();

            CreateMap<DataAccessLayer.Entities.Saving, Api.Contracts.Saving.SavingList>();
            CreateMap<DataAccessLayer.Entities.Saving, Api.Contracts.Saving.SavingDetails>();

            CreateMap<DataAccessLayer.Entities.Salary, Api.Contracts.Salary.SalaryList>();
            CreateMap<DataAccessLayer.Entities.Salary, Api.Contracts.Salary.SalaryDetails>();
            CreateMap<DataAccessLayer.Entities.SalaryDeduction, Api.Contracts.Salary.SalaryDeductionDetails>();

            CreateMap<DataAccessLayer.Entities.Pension, Api.Contracts.Pension.PensionList>();
            CreateMap<DataAccessLayer.Entities.Pension, Api.Contracts.Pension.PensionDetails>();

            CreateMap<DataAccessLayer.Entities.Tax, Api.Contracts.Tax.TaxList>();
            CreateMap<DataAccessLayer.Entities.Tax, Api.Contracts.Tax.TaxDetails>();

            CreateMap<DataAccessLayer.Entities.TaxType, Api.Contracts.TaxType.TaxTypeList>();
            CreateMap<DataAccessLayer.Entities.FrequenceOption, Api.Contracts.FrequenceOption.FrequenceOptionList>();
        }
    }
}
