using AutoMapper;

namespace PersonalFinanceManager.Services.Automapper
{
    public class DTOToModelMapping : Profile
    {
        public DTOToModelMapping()
        {
            CreateMap<PFM.DTOs.UserProfile.UserProfileDetails, Models.UserProfile.UserProfileEditModel>();
            CreateMap<PFM.DTOs.Currency.CurrencyDetails, Models.Currency.CurrencyEditModel>();
            CreateMap<PFM.DTOs.Currency.CurrencyList, Models.Currency.CurrencyListModel>();
            CreateMap<PFM.DTOs.Account.AccountDetails, Models.Account.AccountEditModel>();
            CreateMap<PFM.DTOs.Account.AccountList, Models.Account.AccountListModel>();
            CreateMap<PFM.DTOs.AccountManagement.ImportMovementDetails, Models.AccountManagement.ImportMovementEditModel>();
            CreateMap<PFM.DTOs.AccountManagement.ImportMovementList, Models.AccountManagement.ImportMovementModel>();
            CreateMap<PFM.DTOs.AccountManagement.ImportTypes, Models.AccountManagement.ImportTypes>();
            CreateMap<PFM.DTOs.AccountManagement.MovementPropertyDefinition, Models.AccountManagement.MovementPropertyDefinition>();
            CreateMap<PFM.DTOs.AtmWithdraw.AtmWithdrawDetails, Models.AtmWithdraw.AtmWithdrawEditModel>()
                .ForMember(dest => dest.DateExpenditure, src => src.MapFrom(opts => opts.DateExpense));
            CreateMap<PFM.DTOs.AtmWithdraw.AtmWithdrawList, Models.AtmWithdraw.AtmWithdrawListModel>()
                 .ForMember(dest => dest.DateExpenditure, src => src.MapFrom(opts => opts.DateExpense)); 
            CreateMap<PFM.DTOs.Bank.BankBranchDetails, Models.Bank.BankBrandEditModel>();
            CreateMap<PFM.DTOs.Bank.BankDetails, Models.Bank.BankEditModel>();
            CreateMap<PFM.DTOs.Bank.BankList, Models.Bank.BankListModel>();
            CreateMap<PFM.DTOs.BudgetPlan.BudgetPlanDetails, Models.BudgetPlan.BudgetPlanEditModel>()
                .ForMember(dest => dest.ExpenditureTypes, src => src.MapFrom(opts => opts.ExpenseTypes))
                .ForMember(dest => dest.ExpenditureAverageMonthValue, src => src.MapFrom(opts => opts.ExpenseAverageMonthValue))
                .ForMember(dest => dest.ExpenditurePreviousMonthValue, src => src.MapFrom(opts => opts.ExpensePreviousMonthValue))
                .ForMember(dest => dest.ExpenditureCurrentBudgetPlanValue, src => src.MapFrom(opts => opts.ExpenseCurrentBudgetPlanValue));
            CreateMap<PFM.DTOs.BudgetPlan.BudgetPlanExpenseType, Models.BudgetPlan.BudgetPlanExpenditureType>()
                 .ForMember(dest => dest.ExpenditureType, src => src.MapFrom(opts => opts.ExpenseType));
            CreateMap<PFM.DTOs.BudgetPlan.BudgetPlanList, Models.BudgetPlan.BudgetPlanListModel>();
            CreateMap<PFM.DTOs.Country.CountryDetails, Models.Country.CountryEditModel>();
            CreateMap<PFM.DTOs.Country.CountryList, Models.Country.CountryListModel>();
            CreateMap<PFM.DTOs.Dashboard.ExpenseSummary, Models.Dashboard.ExpenseSummaryModel>();
            CreateMap<PFM.DTOs.Dashboard.ExpenseSummaryByCategory, Models.Dashboard.ExpenseSummaryByCategoryModel>();
            CreateMap<PFM.DTOs.Dashboard.ExpenseSummaryByCategoryAndByMonth, Models.Dashboard.ExpenseSummaryByCategoryAndByMonthModel>();
            CreateMap<PFM.DTOs.Dashboard.ExpenseSummaryByMonth, Models.Dashboard.ExpenseSummaryByMonthModel>();
            CreateMap<PFM.DTOs.Expense.ExpenseDetails, Models.Expenditure.ExpenditureEditModel>()
                .ForMember(dest => dest.DateExpenditure, src => src.MapFrom(opts => opts.DateExpense))
                .ForMember(dest => dest.TypeExpenditureId, src => src.MapFrom(opts => opts.ExpenseTypeId)); 
            CreateMap<PFM.DTOs.Expense.ExpenseList, Models.Expenditure.ExpenditureListModel>()
                .ForMember(dest => dest.DateExpenditure, src => src.MapFrom(opts => opts.DateExpense))
                .ForMember(dest => dest.TypeExpenditureId, src => src.MapFrom(opts => opts.ExpenseTypeId))
                .ForMember(dest => dest.TypeExpenditureName, src => src.MapFrom(opts => opts.ExpenseTypeName));
            CreateMap<PFM.DTOs.ExpenseType.ExpenseTypeDetails, Models.ExpenditureType.ExpenditureTypeEditModel>();
            CreateMap<PFM.DTOs.ExpenseType.ExpenseTypeList, Models.ExpenditureType.ExpenditureTypeListModel>();
            CreateMap<PFM.DTOs.FrequenceOption.FrequenceOptionList, Models.FrequenceOption.FrequenceOptionListModel>();
            CreateMap<PFM.DTOs.Income.IncomeDetails, Models.Income.IncomeEditModel>();
            CreateMap<PFM.DTOs.Income.IncomeList, Models.Income.IncomeListModel>();
            CreateMap<PFM.DTOs.PaymentMethod.PaymentMethodList, Models.PaymentMethod.PaymentMethodListModel>();
            CreateMap<PFM.DTOs.Pension.PensionDetails, Models.Pension.PensionEditModel>();
            CreateMap<PFM.DTOs.Pension.PensionList, Models.Pension.PensionListModel>();
            CreateMap<PFM.DTOs.Salary.SalaryDetails, Models.Salary.SalaryEditModel>();
            CreateMap<PFM.DTOs.Salary.SalaryDeductionDetails, Models.Salary.SalaryDeductionEditModel>();
            CreateMap<PFM.DTOs.Salary.SalaryList, Models.Salary.SalaryListModel>();
            CreateMap<PFM.DTOs.Saving.SavingDetails, Models.Saving.SavingEditModel>();
            CreateMap<PFM.DTOs.Saving.SavingList, Models.Saving.SavingListModel>();
            CreateMap<PFM.DTOs.Tax.TaxDetails, Models.Tax.TaxEditModel>();
            CreateMap<PFM.DTOs.Tax.TaxList, Models.Tax.TaxListModel>();
            CreateMap<PFM.DTOs.TaxType.TaxTypeList, Models.TaxType.TaxTypeListModel>();
            CreateMap<PFM.DTOs.UserProfile.UserProfileDetails, Models.UserProfile.UserProfileEditModel>();
        }
    }
}
