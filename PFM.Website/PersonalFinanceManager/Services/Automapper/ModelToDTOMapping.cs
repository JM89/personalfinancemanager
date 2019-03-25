using AutoMapper;

namespace PersonalFinanceManager.Services.Automapper
{
    public class ModelToDTOMapping : Profile
    {
        public ModelToDTOMapping()
        {
            CreateMap<Models.SearchParameters.ExpenditureGetListSearchParameters, PersonalFinanceManager.DTOs.SearchParameters.ExpenseGetListSearchParameters>();
            CreateMap<Models.Currency.CurrencyEditModel, PersonalFinanceManager.DTOs.Currency.CurrencyDetails>();
            CreateMap<Models.Account.AccountEditModel, PersonalFinanceManager.DTOs.Account.AccountDetails>();
            CreateMap<Models.AccountManagement.ImportMovementEditModel, PersonalFinanceManager.DTOs.AccountManagement.ImportMovementDetails>();
            CreateMap<Models.AtmWithdraw.AtmWithdrawEditModel, PersonalFinanceManager.DTOs.AtmWithdraw.AtmWithdrawDetails>()
                .ForMember(dest => dest.DateExpense, src => src.MapFrom(opts => opts.DateExpenditure));
            CreateMap<Models.Bank.BankEditModel, PersonalFinanceManager.DTOs.Bank.BankDetails>();
            CreateMap<Models.Bank.BankBrandEditModel, PersonalFinanceManager.DTOs.Bank.BankBranchDetails>();
            CreateMap<Models.ExpenditureType.ExpenditureTypeListModel, PersonalFinanceManager.DTOs.ExpenseType.ExpenseTypeList>();
            CreateMap<Models.BudgetPlan.BudgetPlanExpenditureType, PersonalFinanceManager.DTOs.BudgetPlan.BudgetPlanExpenseType>()
                .ForMember(dest => dest.ExpenseType, src => src.MapFrom(opts => opts.ExpenditureType));
            CreateMap<Models.BudgetPlan.BudgetPlanEditModel, PersonalFinanceManager.DTOs.BudgetPlan.BudgetPlanDetails>()
                .ForMember(dest => dest.ExpenseTypes, src => src.MapFrom(opts => opts.ExpenditureTypes));
            CreateMap<Models.Country.CountryEditModel, PersonalFinanceManager.DTOs.Country.CountryDetails>();
            CreateMap<Models.Expenditure.ExpenditureEditModel, PersonalFinanceManager.DTOs.Expense.ExpenseDetails>()
                .ForMember(dest => dest.ExpenseTypeId, src => src.MapFrom(opts => opts.TypeExpenditureId))
                .ForMember(dest => dest.DateExpense, src => src.MapFrom(opts => opts.DateExpenditure));
            CreateMap<Models.ExpenditureType.ExpenditureTypeEditModel, PersonalFinanceManager.DTOs.ExpenseType.ExpenseTypeDetails>();
            CreateMap<Models.Income.IncomeEditModel, PersonalFinanceManager.DTOs.Income.IncomeDetails>();
            CreateMap<Models.Pension.PensionEditModel, PersonalFinanceManager.DTOs.Pension.PensionDetails>();
            CreateMap<Models.Salary.SalaryDeductionEditModel, PersonalFinanceManager.DTOs.Salary.SalaryDeductionDetails>();
            CreateMap<Models.Salary.SalaryEditModel, PersonalFinanceManager.DTOs.Salary.SalaryDetails>();
            CreateMap<Models.Saving.SavingEditModel, PersonalFinanceManager.DTOs.Saving.SavingDetails>();
            CreateMap<Models.Tax.TaxEditModel, PersonalFinanceManager.DTOs.Tax.TaxDetails>();
            CreateMap<Models.UserProfile.UserProfileEditModel, PersonalFinanceManager.DTOs.UserProfile.UserProfileDetails>();
            CreateMap<Models.AspNetUserAccount.LoginViewModel, PersonalFinanceManager.DTOs.UserAccount.User>();
            CreateMap<Models.AspNetUserAccount.RegisterViewModel, PersonalFinanceManager.DTOs.UserAccount.User>();
        }
    }
}
