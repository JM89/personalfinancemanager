using AutoMapper;

namespace PFM.Services.Mappers
{
	public class RequestToModelProfile : Profile
    {
		public RequestToModelProfile()
		{
            CreateMap<Models.ExpenseTypeModel, PFM.Api.Contracts.ExpenseType.ExpenseTypeDetails>();
            CreateMap<Models.ExpenseTypeModel, PFM.Api.Contracts.ExpenseType.ExpenseTypeList>();
            CreateMap<Models.CountryModel, PFM.Bank.Api.Contracts.Country.CountryDetails>();
            CreateMap<Models.BankEditModel, PFM.Bank.Api.Contracts.Bank.BankDetails>();
            CreateMap<Models.BankListModel, PFM.Bank.Api.Contracts.Bank.BankDetails>();
            CreateMap<Models.BankAccountEditModel, PFM.Bank.Api.Contracts.Account.AccountDetails>();
            CreateMap<Models.BankAccountListModel, PFM.Bank.Api.Contracts.Account.AccountDetails>();
            CreateMap<Models.IncomeEditModel, PFM.Api.Contracts.Income.IncomeDetails>();
            CreateMap<Models.IncomeListModel, PFM.Api.Contracts.Income.IncomeList>();
            CreateMap<Models.SavingEditModel, PFM.Api.Contracts.Saving.SavingDetails>();
            CreateMap<Models.SavingListModel, PFM.Api.Contracts.Saving.SavingList>();
            CreateMap<Models.AtmWithdrawEditModel, PFM.Api.Contracts.AtmWithdraw.AtmWithdrawDetails>();
            CreateMap<Models.AtmWithdrawListModel, PFM.Api.Contracts.AtmWithdraw.AtmWithdrawList>();
            CreateMap<Models.ExpenseEditModel, PFM.Api.Contracts.Expense.ExpenseDetails>();
            CreateMap<Models.ExpenseListModel, PFM.Api.Contracts.Expense.ExpenseList>();
            CreateMap<Models.ExpenseSearchParamModel, PFM.Api.Contracts.SearchParameters.ExpenseGetListSearchParameters>();
            CreateMap<Models.MovementSummarySearchParamModel, PFM.Api.Contracts.SearchParameters.MovementSummarySearchParameters>();
            CreateMap<Models.BudgetPlanEditModel, PFM.Api.Contracts.BudgetPlan.BudgetPlanDetails>();
            CreateMap<Models.BudgetPlanExpenseTypeEditModel, PFM.Api.Contracts.BudgetPlan.BudgetPlanExpenseType>();
            CreateMap<Models.BudgetPlanListModel, PFM.Api.Contracts.BudgetPlan.BudgetPlanList>();
        }
    }
}

