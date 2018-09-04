using AutoMapper;

namespace PersonalFinanceManager.Services.Automapper
{
    public class ModelToDTOMapping : Profile
    {
        public ModelToDTOMapping()
        {
            CreateMap<Models.SearchParameters.ExpenditureGetListSearchParameters, PFM.DTOs.SearchParameters.ExpenseGetListSearchParameters>();
            CreateMap<Models.Currency.CurrencyEditModel, PFM.DTOs.Currency.CurrencyDetails>();
            CreateMap<Models.Account.AccountEditModel, PFM.DTOs.Account.AccountDetails>();
            CreateMap<Models.AccountManagement.ImportMovementEditModel, PFM.DTOs.AccountManagement.ImportMovementDetails>();
            CreateMap<Models.AtmWithdraw.AtmWithdrawEditModel, PFM.DTOs.AtmWithdraw.AtmWithdrawDetails>()
                .ForMember(dest => dest.DateExpense, src => src.MapFrom(opts => opts.DateExpenditure));
            CreateMap<Models.Bank.BankEditModel, PFM.DTOs.Bank.BankDetails>();
            CreateMap<Models.Bank.BankBrandEditModel, PFM.DTOs.Bank.BankBranchDetails>();
            CreateMap<Models.ExpenditureType.ExpenditureTypeListModel, PFM.DTOs.ExpenseType.ExpenseTypeList>();
            CreateMap<Models.BudgetPlan.BudgetPlanExpenditureType, PFM.DTOs.BudgetPlan.BudgetPlanExpenseType>()
                .ForMember(dest => dest.ExpenseType, src => src.MapFrom(opts => opts.ExpenditureType));
            CreateMap<Models.BudgetPlan.BudgetPlanEditModel, PFM.DTOs.BudgetPlan.BudgetPlanDetails>()
                .ForMember(dest => dest.ExpenseTypes, src => src.MapFrom(opts => opts.ExpenditureTypes));
            CreateMap<Models.Country.CountryEditModel, PFM.DTOs.Country.CountryDetails>();
            CreateMap<Models.Expenditure.ExpenditureEditModel, PFM.DTOs.Expense.ExpenseDetails>()
                .ForMember(dest => dest.ExpenseTypeId, src => src.MapFrom(opts => opts.TypeExpenditureId))
                .ForMember(dest => dest.DateExpense, src => src.MapFrom(opts => opts.DateExpenditure));
            CreateMap<Models.ExpenditureType.ExpenditureTypeEditModel, PFM.DTOs.ExpenseType.ExpenseTypeDetails>();
            CreateMap<Models.Income.IncomeEditModel, PFM.DTOs.Income.IncomeDetails>();
            CreateMap<Models.Pension.PensionEditModel, PFM.DTOs.Pension.PensionDetails>();
            CreateMap<Models.Salary.SalaryDeductionEditModel, PFM.DTOs.Salary.SalaryDeductionDetails>();
            CreateMap<Models.Salary.SalaryEditModel, PFM.DTOs.Salary.SalaryDetails>();
            CreateMap<Models.Saving.SavingEditModel, PFM.DTOs.Saving.SavingDetails>();
            CreateMap<Models.Tax.TaxEditModel, PFM.DTOs.Tax.TaxDetails>();
            CreateMap<Models.UserProfile.UserProfileEditModel, PFM.DTOs.UserProfile.UserProfileDetails>();
        }
    }
}
