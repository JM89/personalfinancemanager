using AutoMapper;

namespace PersonalFinanceManager.Services.Automapper
{
    public class DTOToModelMapping : Profile
    {
        public DTOToModelMapping()
        {
            CreateMap<PFM.Api.Contracts.UserProfile.UserProfileDetails, Models.UserProfile.UserProfileEditModel>();
            CreateMap<PFM.Api.Contracts.Currency.CurrencyDetails, Models.Currency.CurrencyEditModel>();
            CreateMap<PFM.Api.Contracts.Currency.CurrencyList, Models.Currency.CurrencyListModel>();
            CreateMap<PFM.Api.Contracts.Account.AccountDetails, Models.Account.AccountEditModel>();
            CreateMap<PFM.Api.Contracts.Account.AccountList, Models.Account.AccountListModel>();
            CreateMap<PFM.Api.Contracts.AccountManagement.ImportMovementDetails, Models.AccountManagement.ImportMovementEditModel>();
            CreateMap<PFM.Api.Contracts.AccountManagement.ImportMovementList, Models.AccountManagement.ImportMovementModel>();
            CreateMap<PFM.Api.Contracts.AccountManagement.ImportTypes, Models.AccountManagement.ImportTypes>();
            CreateMap<PFM.Api.Contracts.AccountManagement.MovementPropertyDefinition, Models.AccountManagement.MovementPropertyDefinition>();
            CreateMap<PFM.Api.Contracts.AtmWithdraw.AtmWithdrawDetails, Models.AtmWithdraw.AtmWithdrawEditModel>()
                .ForMember(dest => dest.DateExpenditure, src => src.MapFrom(opts => opts.DateExpense));
            CreateMap<PFM.Api.Contracts.AtmWithdraw.AtmWithdrawList, Models.AtmWithdraw.AtmWithdrawListModel>()
                 .ForMember(dest => dest.DateExpenditure, src => src.MapFrom(opts => opts.DateExpense)); 
            CreateMap<PFM.Api.Contracts.Bank.BankDetails, Models.Bank.BankEditModel>();
            CreateMap<PFM.Api.Contracts.Bank.BankList, Models.Bank.BankListModel>();
            CreateMap<PFM.Api.Contracts.BudgetPlan.BudgetPlanDetails, Models.BudgetPlan.BudgetPlanEditModel>()
                .ForMember(dest => dest.ExpenditureTypes, src => src.MapFrom(opts => opts.ExpenseTypes))
                .ForMember(dest => dest.ExpenditureAverageMonthValue, src => src.MapFrom(opts => opts.ExpenseAverageMonthValue))
                .ForMember(dest => dest.ExpenditurePreviousMonthValue, src => src.MapFrom(opts => opts.ExpensePreviousMonthValue))
                .ForMember(dest => dest.ExpenditureCurrentBudgetPlanValue, src => src.MapFrom(opts => opts.ExpenseCurrentBudgetPlanValue));
            CreateMap<PFM.Api.Contracts.BudgetPlan.BudgetPlanExpenseType, Models.BudgetPlan.BudgetPlanExpenditureType>()
                 .ForMember(dest => dest.ExpenditureType, src => src.MapFrom(opts => opts.ExpenseType));
            CreateMap<PFM.Api.Contracts.BudgetPlan.BudgetPlanList, Models.BudgetPlan.BudgetPlanListModel>();
            CreateMap<PFM.Api.Contracts.Country.CountryDetails, Models.Country.CountryEditModel>();
            CreateMap<PFM.Api.Contracts.Country.CountryList, Models.Country.CountryListModel>();
            CreateMap<PFM.Api.Contracts.Dashboard.ExpenseSummary, Models.Dashboard.ExpenseSummaryModel>();
            CreateMap<PFM.Api.Contracts.Dashboard.ExpenseSummaryByCategory, Models.Dashboard.ExpenseSummaryByCategoryModel>();
            CreateMap<PFM.Api.Contracts.Dashboard.ExpenseSummaryByCategoryAndByMonth, Models.Dashboard.ExpenseSummaryByCategoryAndByMonthModel>();
            CreateMap<PFM.Api.Contracts.Dashboard.ExpenseSummaryByMonth, Models.Dashboard.ExpenseSummaryByMonthModel>();
            CreateMap<PFM.Api.Contracts.Expense.ExpenseDetails, Models.Expenditure.ExpenditureEditModel>()
                .ForMember(dest => dest.DateExpenditure, src => src.MapFrom(opts => opts.DateExpense))
                .ForMember(dest => dest.TypeExpenditureId, src => src.MapFrom(opts => opts.ExpenseTypeId)); 
            CreateMap<PFM.Api.Contracts.Expense.ExpenseList, Models.Expenditure.ExpenditureListModel>()
                .ForMember(dest => dest.DateExpenditure, src => src.MapFrom(opts => opts.DateExpense))
                .ForMember(dest => dest.TypeExpenditureId, src => src.MapFrom(opts => opts.ExpenseTypeId))
                .ForMember(dest => dest.TypeExpenditureName, src => src.MapFrom(opts => opts.ExpenseTypeName));
            CreateMap<PFM.Api.Contracts.ExpenseType.ExpenseTypeDetails, Models.ExpenditureType.ExpenditureTypeEditModel>();
            CreateMap<PFM.Api.Contracts.ExpenseType.ExpenseTypeList, Models.ExpenditureType.ExpenditureTypeListModel>();
            CreateMap<PFM.Api.Contracts.FrequenceOption.FrequenceOptionList, Models.FrequenceOption.FrequenceOptionListModel>();
            CreateMap<PFM.Api.Contracts.Income.IncomeDetails, Models.Income.IncomeEditModel>();
            CreateMap<PFM.Api.Contracts.Income.IncomeList, Models.Income.IncomeListModel>();
            CreateMap<PFM.Api.Contracts.PaymentMethod.PaymentMethodList, Models.PaymentMethod.PaymentMethodListModel>();
            CreateMap<PFM.Api.Contracts.Pension.PensionDetails, Models.Pension.PensionEditModel>();
            CreateMap<PFM.Api.Contracts.Pension.PensionList, Models.Pension.PensionListModel>();
            CreateMap<PFM.Api.Contracts.Salary.SalaryDetails, Models.Salary.SalaryEditModel>();
            CreateMap<PFM.Api.Contracts.Salary.SalaryDeductionDetails, Models.Salary.SalaryDeductionEditModel>();
            CreateMap<PFM.Api.Contracts.Salary.SalaryList, Models.Salary.SalaryListModel>();
            CreateMap<PFM.Api.Contracts.Saving.SavingDetails, Models.Saving.SavingEditModel>();
            CreateMap<PFM.Api.Contracts.Saving.SavingList, Models.Saving.SavingListModel>();
            CreateMap<PFM.Api.Contracts.Tax.TaxDetails, Models.Tax.TaxEditModel>();
            CreateMap<PFM.Api.Contracts.Tax.TaxList, Models.Tax.TaxListModel>();
            CreateMap<PFM.Api.Contracts.TaxType.TaxTypeList, Models.TaxType.TaxTypeListModel>();
            CreateMap<PFM.Api.Contracts.UserProfile.UserProfileDetails, Models.UserProfile.UserProfileEditModel>();
        }
    }
}
