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
        protected override void Configure()
        {
            Mapper.CreateMap<Entities.ExpenditureModel, Models.Expenditure.ExpenditureListModel>();
            Mapper.CreateMap<Entities.ExpenditureModel, Models.Expenditure.ExpenditureEditModel>();

            Mapper.CreateMap<Entities.PaymentMethodModel, Models.PaymentMethod.PaymentMethodListModel>();

            Mapper.CreateMap<Entities.AtmWithdrawModel, Models.AtmWithdraw.AtmWithdrawEditModel>();
            Mapper.CreateMap<Entities.AtmWithdrawModel, Models.AtmWithdraw.AtmWithdrawListModel>();

            Mapper.CreateMap<Entities.ExpenditureTypeModel, Models.ExpenditureType.ExpenditureTypeListModel>();
            Mapper.CreateMap<Entities.ExpenditureTypeModel, Models.ExpenditureType.ExpenditureTypeEditModel>();

            Mapper.CreateMap<Entities.CountryModel, Models.Country.CountryListModel>();
            Mapper.CreateMap<Entities.CountryModel, Models.Country.CountryEditModel>();

            Mapper.CreateMap<Entities.PeriodicOutcomeModel, Models.PeriodicOutcome.PeriodicOutcomeListModel>();
            Mapper.CreateMap<Entities.PeriodicOutcomeModel, Models.PeriodicOutcome.PeriodicOutcomeEditModel>();

            Mapper.CreateMap<Entities.IncomeModel, Models.Income.IncomeListModel>();
            Mapper.CreateMap<Entities.IncomeModel, Models.Income.IncomeEditModel>();

            Mapper.CreateMap<Entities.FrequencyModel, Models.Frequency.FrequencyListModel>();

            Mapper.CreateMap<Entities.BankModel, Models.Bank.BankListModel>();
            Mapper.CreateMap<Entities.BankModel, Models.Bank.BankEditModel>();
            Mapper.CreateMap<Entities.BankBrandModel, Models.Bank.BankBrandEditModel>();

            Mapper.CreateMap<Entities.CurrencyModel, Models.Currency.CurrencyListModel>();
            Mapper.CreateMap<Entities.CurrencyModel, Models.Currency.CurrencyEditModel>();

            Mapper.CreateMap<Entities.AccountModel, Models.Account.AccountListModel>();
            Mapper.CreateMap<Entities.AccountModel, Models.Account.AccountEditModel>();

            Mapper.CreateMap<Entities.BudgetPlanModel, Models.BudgetPlan.BudgetPlanListModel>();
            Mapper.CreateMap<Entities.BudgetPlanModel, Models.BudgetPlan.BudgetPlanEditModel>();
        }
    }
}
