using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Utils.Automapper
{
    public class ModelToEntityMapping : Profile
    {
        public ModelToEntityMapping()
        {
            CreateMap<Models.Expenditure.ExpenditureEditModel, Entities.ExpenditureModel>();
            CreateMap<Models.Expenditure.ExpenditureListModel, Entities.ExpenditureModel>();

            CreateMap<Models.Bank.BankEditModel, Entities.BankModel>();
            CreateMap<Models.Bank.BankBrandEditModel, Entities.BankBrandModel>();

            CreateMap<Models.AtmWithdraw.AtmWithdrawListModel, Entities.AtmWithdrawModel>();
            CreateMap<Models.AtmWithdraw.AtmWithdrawEditModel, Entities.AtmWithdrawModel>();

            CreateMap<Models.PaymentMethod.PaymentMethodListModel, Entities.PaymentMethodModel>();

            CreateMap<Models.Country.CountryListModel, Entities.CountryModel>();
            CreateMap<Models.Country.CountryEditModel, Entities.CountryModel>();

            CreateMap<Models.Income.IncomeListModel, Entities.IncomeModel>();
            CreateMap<Models.Income.IncomeEditModel, Entities.IncomeModel>();

            CreateMap<Models.ExpenditureType.ExpenditureTypeListModel, Entities.ExpenditureTypeModel>();
            CreateMap<Models.ExpenditureType.ExpenditureTypeEditModel, Entities.ExpenditureTypeModel>();

            CreateMap<Models.Currency.CurrencyListModel, Entities.CurrencyModel>();
            CreateMap<Models.Currency.CurrencyEditModel, Entities.CurrencyModel>();

            CreateMap<Models.Account.AccountListModel, Entities.AccountModel>();
            CreateMap<Models.Account.AccountEditModel, Entities.AccountModel>();

            CreateMap<Models.BudgetPlan.BudgetPlanListModel, Entities.BudgetPlanModel>();
            CreateMap<Models.BudgetPlan.BudgetPlanEditModel, Entities.BudgetPlanModel>();

            CreateMap<Models.UserProfile.UserProfileEditModel, Entities.UserProfileModel>();

            CreateMap<Models.Saving.SavingEditModel, Entities.SavingModel>();

            CreateMap<Models.Pension.PensionEditModel, Entities.PensionModel>();

            CreateMap<Models.Salary.SalaryEditModel, Entities.SalaryModel>();
        }
    }
}
