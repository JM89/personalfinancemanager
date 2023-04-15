using AutoMapper;

namespace PersonalFinanceManager.Services.Automapper
{
    public class ModelToDTOMapping : Profile
    {
        public ModelToDTOMapping()
        {
            CreateMap<Models.SearchParameters.ExpenditureGetListSearchParameters, PersonalFinanceManager.Api.Contracts.SearchParameters.ExpenseGetListSearchParameters>();
            CreateMap<Models.Currency.CurrencyEditModel, PersonalFinanceManager.Api.Contracts.Currency.CurrencyDetails>();
            CreateMap<Models.Account.AccountEditModel, PersonalFinanceManager.Api.Contracts.Account.AccountDetails>();
            CreateMap<Models.AccountManagement.ImportMovementEditModel, PersonalFinanceManager.Api.Contracts.AccountManagement.ImportMovementDetails>();
            CreateMap<Models.AtmWithdraw.AtmWithdrawEditModel, PersonalFinanceManager.Api.Contracts.AtmWithdraw.AtmWithdrawDetails>()
                .ForMember(dest => dest.DateExpense, src => src.MapFrom(opts => opts.DateExpenditure));
            CreateMap<Models.Bank.BankEditModel, PersonalFinanceManager.Api.Contracts.Bank.BankDetails>();
            CreateMap<Models.Bank.BankBrandEditModel, PersonalFinanceManager.Api.Contracts.Bank.BankBranchDetails>();
            CreateMap<Models.ExpenditureType.ExpenditureTypeListModel, PersonalFinanceManager.Api.Contracts.ExpenseType.ExpenseTypeList>();
            CreateMap<Models.BudgetPlan.BudgetPlanExpenditureType, PersonalFinanceManager.Api.Contracts.BudgetPlan.BudgetPlanExpenseType>()
                .ForMember(dest => dest.ExpenseType, src => src.MapFrom(opts => opts.ExpenditureType));
            CreateMap<Models.BudgetPlan.BudgetPlanEditModel, PersonalFinanceManager.Api.Contracts.BudgetPlan.BudgetPlanDetails>()
                .ForMember(dest => dest.ExpenseTypes, src => src.MapFrom(opts => opts.ExpenditureTypes));
            CreateMap<Models.Country.CountryEditModel, PersonalFinanceManager.Api.Contracts.Country.CountryDetails>();
            CreateMap<Models.Expenditure.ExpenditureEditModel, PersonalFinanceManager.Api.Contracts.Expense.ExpenseDetails>()
                .ForMember(dest => dest.ExpenseTypeId, src => src.MapFrom(opts => opts.TypeExpenditureId))
                .ForMember(dest => dest.DateExpense, src => src.MapFrom(opts => opts.DateExpenditure));
            CreateMap<Models.ExpenditureType.ExpenditureTypeEditModel, PersonalFinanceManager.Api.Contracts.ExpenseType.ExpenseTypeDetails>();
            CreateMap<Models.Income.IncomeEditModel, PersonalFinanceManager.Api.Contracts.Income.IncomeDetails>();
            CreateMap<Models.Pension.PensionEditModel, PersonalFinanceManager.Api.Contracts.Pension.PensionDetails>();
            CreateMap<Models.Salary.SalaryDeductionEditModel, PersonalFinanceManager.Api.Contracts.Salary.SalaryDeductionDetails>();
            CreateMap<Models.Salary.SalaryEditModel, PersonalFinanceManager.Api.Contracts.Salary.SalaryDetails>();
            CreateMap<Models.Saving.SavingEditModel, PersonalFinanceManager.Api.Contracts.Saving.SavingDetails>();
            CreateMap<Models.Tax.TaxEditModel, PersonalFinanceManager.Api.Contracts.Tax.TaxDetails>();
            CreateMap<Models.UserProfile.UserProfileEditModel, PersonalFinanceManager.Api.Contracts.UserProfile.UserProfileDetails>();
            CreateMap<Models.AspNetUserAccount.LoginViewModel, PersonalFinanceManager.Api.Contracts.UserAccount.User>();
            CreateMap<Models.AspNetUserAccount.RegisterViewModel, PersonalFinanceManager.Api.Contracts.UserAccount.User>();
        }
    }
}
