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
            CreateMap<DataAccessLayer.Entities.Expense, DTOs.Expense.ExpenseList>();
            CreateMap<DataAccessLayer.Entities.Expense, DTOs.Expense.ExpenseDetails>();

            CreateMap<DataAccessLayer.Entities.PaymentMethod, DTOs.PaymentMethod.PaymentMethodList>();

            CreateMap<DataAccessLayer.Entities.AtmWithdraw, DTOs.AtmWithdraw.AtmWithdrawDetails>();
            CreateMap<DataAccessLayer.Entities.AtmWithdraw, DTOs.AtmWithdraw.AtmWithdrawList>();

            CreateMap<DataAccessLayer.Entities.ExpenseType, DTOs.ExpenseType.ExpenseTypeList>();
            CreateMap<DataAccessLayer.Entities.ExpenseType, DTOs.ExpenseType.ExpenseTypeDetails>();

            CreateMap<DataAccessLayer.Entities.Country, DTOs.Country.CountryList>();
            CreateMap<DataAccessLayer.Entities.Country, DTOs.Country.CountryDetails>();

            CreateMap<DataAccessLayer.Entities.Income, DTOs.Income.IncomeList>();
            CreateMap<DataAccessLayer.Entities.Income, DTOs.Income.IncomeDetails>();

            CreateMap<DataAccessLayer.Entities.Bank, DTOs.Bank.BankList>();
            CreateMap<DataAccessLayer.Entities.Bank, DTOs.Bank.BankDetails>();
            CreateMap<DataAccessLayer.Entities.BankBranch, DTOs.Bank.BankBranchDetails>();

            CreateMap<DataAccessLayer.Entities.Currency, DTOs.Currency.CurrencyList>();
            CreateMap<DataAccessLayer.Entities.Currency, DTOs.Currency.CurrencyDetails>();

            CreateMap<DataAccessLayer.Entities.Account, DTOs.Account.AccountList>();
            CreateMap<DataAccessLayer.Entities.Account, DTOs.Account.AccountDetails>();

            CreateMap<DataAccessLayer.Entities.BudgetPlan, DTOs.BudgetPlan.BudgetPlanList>();
            CreateMap<DataAccessLayer.Entities.BudgetPlan, DTOs.BudgetPlan.BudgetPlanDetails>();

            CreateMap<DataAccessLayer.Entities.UserProfile, DTOs.UserProfile.UserProfileDetails>();

            CreateMap<DataAccessLayer.Entities.Saving, DTOs.Saving.SavingList>();
            CreateMap<DataAccessLayer.Entities.Saving, DTOs.Saving.SavingDetails>();

            CreateMap<DataAccessLayer.Entities.Salary, DTOs.Salary.SalaryList>();
            CreateMap<DataAccessLayer.Entities.Salary, DTOs.Salary.SalaryDetails>();
            CreateMap<DataAccessLayer.Entities.SalaryDeduction, DTOs.Salary.SalaryDeductionDetails>();

            CreateMap<DataAccessLayer.Entities.Pension, DTOs.Pension.PensionList>();
            CreateMap<DataAccessLayer.Entities.Pension, DTOs.Pension.PensionDetails>();

            CreateMap<DataAccessLayer.Entities.Tax, DTOs.Tax.TaxList>();
            CreateMap<DataAccessLayer.Entities.Tax, DTOs.Tax.TaxDetails>();

            CreateMap<DataAccessLayer.Entities.TaxType, DTOs.TaxType.TaxTypeList>();
            CreateMap<DataAccessLayer.Entities.FrequenceOption, DTOs.FrequenceOption.FrequenceOptionList>();
        }
    }
}
