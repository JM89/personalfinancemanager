using AutoMapper;

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
            CreateMap<PFM.Bank.Api.Contracts.Account.AccountList, Models.BankAccountListModel>();
            CreateMap<PFM.Bank.Api.Contracts.Account.AccountDetails, Models.BankAccountListModel>();
            CreateMap<PFM.Bank.Api.Contracts.Account.AccountDetails, Models.BankAccountEditModel>();
            CreateMap<PFM.Api.Contracts.Income.IncomeList, Models.IncomeListModel>();
            CreateMap<PFM.Api.Contracts.Income.IncomeDetails, Models.IncomeEditModel>();
            CreateMap<PFM.Api.Contracts.Saving.SavingList, Models.SavingListModel>();
            CreateMap<PFM.Api.Contracts.Saving.SavingDetails, Models.SavingEditModel>();
            CreateMap<PFM.Api.Contracts.AtmWithdraw.AtmWithdrawList, Models.AtmWithdrawListModel>();
            CreateMap<PFM.Api.Contracts.AtmWithdraw.AtmWithdrawDetails, Models.AtmWithdrawEditModel>();
            CreateMap<PFM.Api.Contracts.Expense.ExpenseList, Models.ExpenseListModel>();
            CreateMap<PFM.Api.Contracts.Expense.ExpenseDetails, Models.ExpenseEditModel>();
            CreateMap<PFM.Api.Contracts.PaymentMethod.PaymentMethodList, Models.PaymentMethodListModel>();
            CreateMap<PFM.Api.Contracts.BudgetPlan.BudgetPlanList, Models.BudgetPlanListModel>();
            CreateMap<PFM.Api.Contracts.BudgetPlan.BudgetPlanDetails, Models.BudgetPlanEditModel>();
            CreateMap<PFM.Api.Contracts.BudgetPlan.BudgetPlanExpenseType, Models.BudgetPlanExpenseTypeEditModel>();
        }
    }
}

