using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Utils.Automapper
{
    public class EntityToModelMapping : Profile
    {
        public EntityToModelMapping()
        {
            CreateMap<Entities.ExpenditureModel, Models.Expenditure.ExpenditureListModel>();
            CreateMap<Entities.ExpenditureModel, Models.Expenditure.ExpenditureEditModel>();

            CreateMap<Entities.PaymentMethodModel, Models.PaymentMethod.PaymentMethodListModel>();

            CreateMap<Entities.AtmWithdrawModel, Models.AtmWithdraw.AtmWithdrawEditModel>();
            CreateMap<Entities.AtmWithdrawModel, Models.AtmWithdraw.AtmWithdrawListModel>();

            CreateMap<Entities.ExpenditureTypeModel, Models.ExpenditureType.ExpenditureTypeListModel>();
            CreateMap<Entities.ExpenditureTypeModel, Models.ExpenditureType.ExpenditureTypeEditModel>();

            CreateMap<Entities.CountryModel, Models.Country.CountryListModel>();
            CreateMap<Entities.CountryModel, Models.Country.CountryEditModel>();

            CreateMap<Entities.IncomeModel, Models.Income.IncomeListModel>();
            CreateMap<Entities.IncomeModel, Models.Income.IncomeEditModel>();

            CreateMap<Entities.BankModel, Models.Bank.BankListModel>();
            CreateMap<Entities.BankModel, Models.Bank.BankEditModel>();
            CreateMap<Entities.BankBrandModel, Models.Bank.BankBrandEditModel>();

            CreateMap<Entities.CurrencyModel, Models.Currency.CurrencyListModel>();
            CreateMap<Entities.CurrencyModel, Models.Currency.CurrencyEditModel>();

            CreateMap<Entities.AccountModel, Models.Account.AccountListModel>();
            CreateMap<Entities.AccountModel, Models.Account.AccountEditModel>();

            CreateMap<Entities.BudgetPlanModel, Models.BudgetPlan.BudgetPlanListModel>();
            CreateMap<Entities.BudgetPlanModel, Models.BudgetPlan.BudgetPlanEditModel>();

            CreateMap<Entities.UserProfileModel, Models.UserProfile.UserProfileEditModel>();

            CreateMap<Entities.SavingModel, Models.Saving.SavingListModel>();
            CreateMap<Entities.SavingModel, Models.Saving.SavingEditModel>();

            CreateMap<Entities.SalaryModel, Models.Salary.SalaryListModel>();
            CreateMap<Entities.SalaryModel, Models.Salary.SalaryEditModel>();

            CreateMap<Entities.PensionModel, Models.Pension.PensionListModel>();
            CreateMap<Entities.PensionModel, Models.Pension.PensionEditModel>();
        }
    }
}
