using AutoMapper;

namespace PersonalFinanceManager.Services.Automapper
{
    public class DTOToModelMapping : Profile
    {
        public DTOToModelMapping()
        {
            CreateMap<PersonalFinanceManager.DTOs.UserProfile.UserProfileDetails, Models.UserProfile.UserProfileEditModel>();
            CreateMap<PersonalFinanceManager.DTOs.Currency.CurrencyDetails, Models.Currency.CurrencyEditModel>();
            CreateMap<PersonalFinanceManager.DTOs.Currency.CurrencyList, Models.Currency.CurrencyListModel>();
            CreateMap<PersonalFinanceManager.DTOs.Account.AccountDetails, Models.Account.AccountEditModel>();
            CreateMap<PersonalFinanceManager.DTOs.Account.AccountList, Models.Account.AccountListModel>();
            CreateMap<PersonalFinanceManager.DTOs.AccountManagement.ImportMovementDetails, Models.AccountManagement.ImportMovementEditModel>();
            CreateMap<PersonalFinanceManager.DTOs.AccountManagement.ImportMovementList, Models.AccountManagement.ImportMovementModel>();
            CreateMap<PersonalFinanceManager.DTOs.AccountManagement.ImportTypes, Models.AccountManagement.ImportTypes>();
            CreateMap<PersonalFinanceManager.DTOs.AccountManagement.MovementPropertyDefinition, Models.AccountManagement.MovementPropertyDefinition>();
            CreateMap<PersonalFinanceManager.DTOs.AtmWithdraw.AtmWithdrawDetails, Models.AtmWithdraw.AtmWithdrawEditModel>()
                .ForMember(dest => dest.DateExpenditure, src => src.MapFrom(opts => opts.DateExpense));
            CreateMap<PersonalFinanceManager.DTOs.AtmWithdraw.AtmWithdrawList, Models.AtmWithdraw.AtmWithdrawListModel>()
                 .ForMember(dest => dest.DateExpenditure, src => src.MapFrom(opts => opts.DateExpense)); 
            CreateMap<PersonalFinanceManager.DTOs.Bank.BankBranchDetails, Models.Bank.BankBrandEditModel>();
            CreateMap<PersonalFinanceManager.DTOs.Bank.BankDetails, Models.Bank.BankEditModel>();
            CreateMap<PersonalFinanceManager.DTOs.Bank.BankList, Models.Bank.BankListModel>();
            CreateMap<PersonalFinanceManager.DTOs.BudgetPlan.BudgetPlanDetails, Models.BudgetPlan.BudgetPlanEditModel>()
                .ForMember(dest => dest.ExpenditureTypes, src => src.MapFrom(opts => opts.ExpenseTypes))
                .ForMember(dest => dest.ExpenditureAverageMonthValue, src => src.MapFrom(opts => opts.ExpenseAverageMonthValue))
                .ForMember(dest => dest.ExpenditurePreviousMonthValue, src => src.MapFrom(opts => opts.ExpensePreviousMonthValue))
                .ForMember(dest => dest.ExpenditureCurrentBudgetPlanValue, src => src.MapFrom(opts => opts.ExpenseCurrentBudgetPlanValue));
            CreateMap<PersonalFinanceManager.DTOs.BudgetPlan.BudgetPlanExpenseType, Models.BudgetPlan.BudgetPlanExpenditureType>()
                 .ForMember(dest => dest.ExpenditureType, src => src.MapFrom(opts => opts.ExpenseType));
            CreateMap<PersonalFinanceManager.DTOs.BudgetPlan.BudgetPlanList, Models.BudgetPlan.BudgetPlanListModel>();
            CreateMap<PersonalFinanceManager.DTOs.Country.CountryDetails, Models.Country.CountryEditModel>();
            CreateMap<PersonalFinanceManager.DTOs.Country.CountryList, Models.Country.CountryListModel>();
            CreateMap<PersonalFinanceManager.DTOs.Dashboard.ExpenseSummary, Models.Dashboard.ExpenseSummaryModel>();
            CreateMap<PersonalFinanceManager.DTOs.Dashboard.ExpenseSummaryByCategory, Models.Dashboard.ExpenseSummaryByCategoryModel>();
            CreateMap<PersonalFinanceManager.DTOs.Dashboard.ExpenseSummaryByCategoryAndByMonth, Models.Dashboard.ExpenseSummaryByCategoryAndByMonthModel>();
            CreateMap<PersonalFinanceManager.DTOs.Dashboard.ExpenseSummaryByMonth, Models.Dashboard.ExpenseSummaryByMonthModel>();
            CreateMap<PersonalFinanceManager.DTOs.Expense.ExpenseDetails, Models.Expenditure.ExpenditureEditModel>()
                .ForMember(dest => dest.DateExpenditure, src => src.MapFrom(opts => opts.DateExpense))
                .ForMember(dest => dest.TypeExpenditureId, src => src.MapFrom(opts => opts.ExpenseTypeId)); 
            CreateMap<PersonalFinanceManager.DTOs.Expense.ExpenseList, Models.Expenditure.ExpenditureListModel>()
                .ForMember(dest => dest.DateExpenditure, src => src.MapFrom(opts => opts.DateExpense))
                .ForMember(dest => dest.TypeExpenditureId, src => src.MapFrom(opts => opts.ExpenseTypeId))
                .ForMember(dest => dest.TypeExpenditureName, src => src.MapFrom(opts => opts.ExpenseTypeName));
            CreateMap<PersonalFinanceManager.DTOs.ExpenseType.ExpenseTypeDetails, Models.ExpenditureType.ExpenditureTypeEditModel>();
            CreateMap<PersonalFinanceManager.DTOs.ExpenseType.ExpenseTypeList, Models.ExpenditureType.ExpenditureTypeListModel>();
            CreateMap<PersonalFinanceManager.DTOs.FrequenceOption.FrequenceOptionList, Models.FrequenceOption.FrequenceOptionListModel>();
            CreateMap<PersonalFinanceManager.DTOs.Income.IncomeDetails, Models.Income.IncomeEditModel>();
            CreateMap<PersonalFinanceManager.DTOs.Income.IncomeList, Models.Income.IncomeListModel>();
            CreateMap<PFM.Services.DTOs.PaymentMethod.PaymentMethodList, Models.PaymentMethod.PaymentMethodListModel>();
            CreateMap<PersonalFinanceManager.DTOs.Pension.PensionDetails, Models.Pension.PensionEditModel>();
            CreateMap<PersonalFinanceManager.DTOs.Pension.PensionList, Models.Pension.PensionListModel>();
            CreateMap<PersonalFinanceManager.DTOs.Salary.SalaryDetails, Models.Salary.SalaryEditModel>();
            CreateMap<PersonalFinanceManager.DTOs.Salary.SalaryDeductionDetails, Models.Salary.SalaryDeductionEditModel>();
            CreateMap<PersonalFinanceManager.DTOs.Salary.SalaryList, Models.Salary.SalaryListModel>();
            CreateMap<PersonalFinanceManager.DTOs.Saving.SavingDetails, Models.Saving.SavingEditModel>();
            CreateMap<PersonalFinanceManager.DTOs.Saving.SavingList, Models.Saving.SavingListModel>();
            CreateMap<PersonalFinanceManager.DTOs.Tax.TaxDetails, Models.Tax.TaxEditModel>();
            CreateMap<PersonalFinanceManager.DTOs.Tax.TaxList, Models.Tax.TaxListModel>();
            CreateMap<PersonalFinanceManager.DTOs.TaxType.TaxTypeList, Models.TaxType.TaxTypeListModel>();
            CreateMap<PersonalFinanceManager.DTOs.UserProfile.UserProfileDetails, Models.UserProfile.UserProfileEditModel>();
            CreateMap<PersonalFinanceManager.DTOs.UserAccount.AuthenticatedUser, Models.AspNetUserAccount.AuthenticatedUser>();
        }
    }
}
