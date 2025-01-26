using AutoMapper;
using PFM.Services.Utils;

namespace PFM.Services.Mappers
{
	public class ModelToResponseProfile : Profile
    {
		public ModelToResponseProfile()
		{
            CreateMap<PFM.Api.Contracts.ExpenseType.ExpenseTypeList, Models.ExpenseTypeModel>();
            CreateMap<PFM.Api.Contracts.ExpenseType.ExpenseTypeDetails, Models.ExpenseTypeModel>();
            CreateMap<PFM.Bank.Api.Contracts.Country.CountryList, Models.CountryModel>();
            CreateMap<PFM.Bank.Api.Contracts.Country.CountryDetails, Models.CountryModel>();
            CreateMap<PFM.Bank.Api.Contracts.Bank.BankList, Models.BankListModel>();
            CreateMap<PFM.Bank.Api.Contracts.Bank.BankDetails, Models.BankEditModel>();
            CreateMap<PFM.Bank.Api.Contracts.Currency.CurrencyList, Models.CurrencyModel>();
            CreateMap<PFM.Bank.Api.Contracts.Currency.CurrencyDetails, Models.CurrencyModel>();
            CreateMap<PFM.Bank.Api.Contracts.Account.AccountList, Models.BankAccountListModel>()
	            .ForMember(x => x.DisplayedInitialBalance, map => map.MapFrom(src => DecimalFormatHelper.GetDisplayDecimalValue(src.InitialBalance, src.CurrencySymbol)))
	            .ForMember(x => x.DisplayedCurrentBalance, map => map.MapFrom(src => DecimalFormatHelper.GetDisplayDecimalValue(src.CurrentBalance, src.CurrencySymbol)));
            CreateMap<PFM.Bank.Api.Contracts.Account.AccountDetails, Models.BankAccountListModel>()
	            .ForMember(x => x.DisplayedInitialBalance, map => map.MapFrom(src => DecimalFormatHelper.GetDisplayDecimalValue(src.InitialBalance, src.CurrencySymbol)))
	            .ForMember(x => x.DisplayedCurrentBalance, map => map.MapFrom(src => DecimalFormatHelper.GetDisplayDecimalValue(src.CurrentBalance, src.CurrencySymbol)));
            CreateMap<PFM.Bank.Api.Contracts.Account.AccountDetails, Models.BankAccountEditModel>()
	            .ForMember(x => x.DisplayedCurrentBalance, map => map.MapFrom(src => DecimalFormatHelper.GetDisplayDecimalValue(src.CurrentBalance, src.CurrencySymbol)));
            CreateMap<PFM.Api.Contracts.Income.IncomeList, Models.IncomeListModel>();
            CreateMap<PFM.Api.Contracts.Income.IncomeDetails, Models.IncomeEditModel>();
            CreateMap<PFM.Api.Contracts.Saving.SavingList, Models.SavingListModel>()
	            .ForMember(x => x.DisplayedAmount, map => map.MapFrom(src => DecimalFormatHelper.GetDisplayDecimalValue(src.Amount, src.AccountCurrencySymbol)))
	            .ForMember(x => x.DateSaving, map => map.MapFrom(src => DateTimeFormatHelper.GetDisplayDateValue(src.DateSaving)));
            CreateMap<PFM.Api.Contracts.Saving.SavingDetails, Models.SavingEditModel>()
	            .ForMember(x => x.DateSaving, map => map.MapFrom(src => DateTimeFormatHelper.GetDisplayDateValue(src.DateSaving)));
            CreateMap<PFM.Api.Contracts.AtmWithdraw.AtmWithdrawList, Models.AtmWithdrawListModel>();
            CreateMap<PFM.Api.Contracts.AtmWithdraw.AtmWithdrawDetails, Models.AtmWithdrawEditModel>();
            CreateMap<PFM.Api.Contracts.Expense.ExpenseList, Models.ExpenseListModel>();
            CreateMap<PFM.Api.Contracts.Expense.ExpenseDetails, Models.ExpenseEditModel>()
	            .ForMember(x => x.DisplayedDateExpense, map => map.MapFrom(src => DateTimeFormatHelper.GetDisplayDateValue(src.DateExpense)));
            CreateMap<PFM.Api.Contracts.PaymentMethod.PaymentMethodList, Models.PaymentMethodListModel>();
            CreateMap<PFM.Api.Contracts.BudgetPlan.BudgetPlanList, Models.BudgetPlanListModel>();
            CreateMap<PFM.Api.Contracts.BudgetPlan.BudgetPlanDetails, Models.BudgetPlanEditModel>();
            CreateMap<PFM.Api.Contracts.BudgetPlan.BudgetPlanExpenseType, Models.BudgetPlanExpenseTypeEditModel>();
        }
    }
}

