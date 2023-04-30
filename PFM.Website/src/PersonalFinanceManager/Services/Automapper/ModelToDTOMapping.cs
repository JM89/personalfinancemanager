using AutoMapper;

namespace PersonalFinanceManager.Services.Automapper
{
    public class ModelToDTOMapping : Profile
    {
        public ModelToDTOMapping()
        {
            CreateMap<Models.SearchParameters.ExpenditureGetListSearchParameters, PFM.Api.Contracts.SearchParameters.ExpenseGetListSearchParameters>();
            CreateMap<Models.Currency.CurrencyEditModel, PFM.Api.Contracts.Currency.CurrencyDetails>();
            CreateMap<Models.Account.AccountEditModel, PFM.Api.Contracts.Account.AccountDetails>();
            CreateMap<Models.AccountManagement.ImportMovementEditModel, PFM.Api.Contracts.AccountManagement.ImportMovementDetails>();
            CreateMap<Models.AtmWithdraw.AtmWithdrawEditModel, PFM.Api.Contracts.AtmWithdraw.AtmWithdrawDetails>()
                .ForMember(dest => dest.DateExpense, src => src.MapFrom(opts => opts.DateExpenditure));
            CreateMap<Models.Bank.BankEditModel, PFM.Api.Contracts.Bank.BankDetails>();
            CreateMap<Models.ExpenditureType.ExpenditureTypeListModel, PFM.Api.Contracts.ExpenseType.ExpenseTypeList>();
            CreateMap<Models.BudgetPlan.BudgetPlanExpenditureType, PFM.Api.Contracts.BudgetPlan.BudgetPlanExpenseType>()
                .ForMember(dest => dest.ExpenseType, src => src.MapFrom(opts => opts.ExpenditureType));
            CreateMap<Models.BudgetPlan.BudgetPlanEditModel, PFM.Api.Contracts.BudgetPlan.BudgetPlanDetails>()
                .ForMember(dest => dest.ExpenseTypes, src => src.MapFrom(opts => opts.ExpenditureTypes));
            CreateMap<Models.Country.CountryEditModel, PFM.Api.Contracts.Country.CountryDetails>();
            CreateMap<Models.Expenditure.ExpenditureEditModel, PFM.Api.Contracts.Expense.ExpenseDetails>()
                .ForMember(dest => dest.ExpenseTypeId, src => src.MapFrom(opts => opts.TypeExpenditureId))
                .ForMember(dest => dest.DateExpense, src => src.MapFrom(opts => opts.DateExpenditure));
            CreateMap<Models.ExpenditureType.ExpenditureTypeEditModel, PFM.Api.Contracts.ExpenseType.ExpenseTypeDetails>();
            CreateMap<Models.Income.IncomeEditModel, PFM.Api.Contracts.Income.IncomeDetails>();
            CreateMap<Models.Pension.PensionEditModel, PFM.Api.Contracts.Pension.PensionDetails>();
            CreateMap<Models.Salary.SalaryDeductionEditModel, PFM.Api.Contracts.Salary.SalaryDeductionDetails>();
            CreateMap<Models.Salary.SalaryEditModel, PFM.Api.Contracts.Salary.SalaryDetails>();
            CreateMap<Models.Saving.SavingEditModel, PFM.Api.Contracts.Saving.SavingDetails>();
            CreateMap<Models.Tax.TaxEditModel, PFM.Api.Contracts.Tax.TaxDetails>();
            CreateMap<Models.UserProfile.UserProfileEditModel, PFM.Api.Contracts.UserProfile.UserProfileDetails>();
        }
    }
}
