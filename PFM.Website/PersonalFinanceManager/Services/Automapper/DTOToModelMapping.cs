using AutoMapper;

namespace PersonalFinanceManager.Services.Automapper
{
    public class DTOToModelMapping : Profile
    {
        public DTOToModelMapping()
        {
            CreateMap<PersonalFinanceManager.Api.Contracts.UserProfile.UserProfileDetails, Models.UserProfile.UserProfileEditModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.Currency.CurrencyDetails, Models.Currency.CurrencyEditModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.Currency.CurrencyList, Models.Currency.CurrencyListModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.Account.AccountDetails, Models.Account.AccountEditModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.Account.AccountList, Models.Account.AccountListModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.AccountManagement.ImportMovementDetails, Models.AccountManagement.ImportMovementEditModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.AccountManagement.ImportMovementList, Models.AccountManagement.ImportMovementModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.AccountManagement.ImportTypes, Models.AccountManagement.ImportTypes>();
            CreateMap<PersonalFinanceManager.Api.Contracts.AccountManagement.MovementPropertyDefinition, Models.AccountManagement.MovementPropertyDefinition>();
            CreateMap<PersonalFinanceManager.Api.Contracts.AtmWithdraw.AtmWithdrawDetails, Models.AtmWithdraw.AtmWithdrawEditModel>()
                .ForMember(dest => dest.DateExpenditure, src => src.MapFrom(opts => opts.DateExpense));
            CreateMap<PersonalFinanceManager.Api.Contracts.AtmWithdraw.AtmWithdrawList, Models.AtmWithdraw.AtmWithdrawListModel>()
                 .ForMember(dest => dest.DateExpenditure, src => src.MapFrom(opts => opts.DateExpense)); 
            CreateMap<PersonalFinanceManager.Api.Contracts.Bank.BankBranchDetails, Models.Bank.BankBrandEditModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.Bank.BankDetails, Models.Bank.BankEditModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.Bank.BankList, Models.Bank.BankListModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.BudgetPlan.BudgetPlanDetails, Models.BudgetPlan.BudgetPlanEditModel>()
                .ForMember(dest => dest.ExpenditureTypes, src => src.MapFrom(opts => opts.ExpenseTypes))
                .ForMember(dest => dest.ExpenditureAverageMonthValue, src => src.MapFrom(opts => opts.ExpenseAverageMonthValue))
                .ForMember(dest => dest.ExpenditurePreviousMonthValue, src => src.MapFrom(opts => opts.ExpensePreviousMonthValue))
                .ForMember(dest => dest.ExpenditureCurrentBudgetPlanValue, src => src.MapFrom(opts => opts.ExpenseCurrentBudgetPlanValue));
            CreateMap<PersonalFinanceManager.Api.Contracts.BudgetPlan.BudgetPlanExpenseType, Models.BudgetPlan.BudgetPlanExpenditureType>()
                 .ForMember(dest => dest.ExpenditureType, src => src.MapFrom(opts => opts.ExpenseType));
            CreateMap<PersonalFinanceManager.Api.Contracts.BudgetPlan.BudgetPlanList, Models.BudgetPlan.BudgetPlanListModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.Country.CountryDetails, Models.Country.CountryEditModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.Country.CountryList, Models.Country.CountryListModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.Dashboard.ExpenseSummary, Models.Dashboard.ExpenseSummaryModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.Dashboard.ExpenseSummaryByCategory, Models.Dashboard.ExpenseSummaryByCategoryModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.Dashboard.ExpenseSummaryByCategoryAndByMonth, Models.Dashboard.ExpenseSummaryByCategoryAndByMonthModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.Dashboard.ExpenseSummaryByMonth, Models.Dashboard.ExpenseSummaryByMonthModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.Expense.ExpenseDetails, Models.Expenditure.ExpenditureEditModel>()
                .ForMember(dest => dest.DateExpenditure, src => src.MapFrom(opts => opts.DateExpense))
                .ForMember(dest => dest.TypeExpenditureId, src => src.MapFrom(opts => opts.ExpenseTypeId)); 
            CreateMap<PersonalFinanceManager.Api.Contracts.Expense.ExpenseList, Models.Expenditure.ExpenditureListModel>()
                .ForMember(dest => dest.DateExpenditure, src => src.MapFrom(opts => opts.DateExpense))
                .ForMember(dest => dest.TypeExpenditureId, src => src.MapFrom(opts => opts.ExpenseTypeId))
                .ForMember(dest => dest.TypeExpenditureName, src => src.MapFrom(opts => opts.ExpenseTypeName));
            CreateMap<PersonalFinanceManager.Api.Contracts.ExpenseType.ExpenseTypeDetails, Models.ExpenditureType.ExpenditureTypeEditModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.ExpenseType.ExpenseTypeList, Models.ExpenditureType.ExpenditureTypeListModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.FrequenceOption.FrequenceOptionList, Models.FrequenceOption.FrequenceOptionListModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.Income.IncomeDetails, Models.Income.IncomeEditModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.Income.IncomeList, Models.Income.IncomeListModel>();
            CreateMap<PFM.Api.Contracts.PaymentMethod.PaymentMethodList, Models.PaymentMethod.PaymentMethodListModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.Pension.PensionDetails, Models.Pension.PensionEditModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.Pension.PensionList, Models.Pension.PensionListModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.Salary.SalaryDetails, Models.Salary.SalaryEditModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.Salary.SalaryDeductionDetails, Models.Salary.SalaryDeductionEditModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.Salary.SalaryList, Models.Salary.SalaryListModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.Saving.SavingDetails, Models.Saving.SavingEditModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.Saving.SavingList, Models.Saving.SavingListModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.Tax.TaxDetails, Models.Tax.TaxEditModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.Tax.TaxList, Models.Tax.TaxListModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.TaxType.TaxTypeList, Models.TaxType.TaxTypeListModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.UserProfile.UserProfileDetails, Models.UserProfile.UserProfileEditModel>();
            CreateMap<PersonalFinanceManager.Api.Contracts.UserAccount.AuthenticatedUser, Models.AspNetUserAccount.AuthenticatedUser>();
        }
    }
}
