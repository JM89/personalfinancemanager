using AutoMapper;

namespace PFM.Services.Core.Automapper
{
    public class EntityToModelMapping : Profile
    {
        public EntityToModelMapping()
        {
            CreateMap<DataAccessLayer.Entities.Expense, Api.Contracts.Expense.ExpenseList>();
            CreateMap<DataAccessLayer.Entities.Expense, Api.Contracts.Expense.ExpenseDetails>();
            
            CreateMap<DataAccessLayer.Entities.PaymentMethod, Api.Contracts.PaymentMethod.PaymentMethodList>();

            CreateMap<DataAccessLayer.Entities.AtmWithdraw, Api.Contracts.AtmWithdraw.AtmWithdrawDetails>();
            CreateMap<DataAccessLayer.Entities.AtmWithdraw, Api.Contracts.AtmWithdraw.AtmWithdrawList>();

            CreateMap<DataAccessLayer.Entities.ExpenseType, Api.Contracts.ExpenseType.ExpenseTypeList>();
            CreateMap<DataAccessLayer.Entities.ExpenseType, Api.Contracts.ExpenseType.ExpenseTypeDetails>()
                .ForMember(a => a.OwnerId, opt => opt.MapFrom(x => x.User_Id)); 

            CreateMap<DataAccessLayer.Entities.Income, Api.Contracts.Income.IncomeList>();
            CreateMap<DataAccessLayer.Entities.Income, Api.Contracts.Income.IncomeDetails>();

            CreateMap<DataAccessLayer.Entities.BudgetPlan, Api.Contracts.BudgetPlan.BudgetPlanList>();
            CreateMap<DataAccessLayer.Entities.BudgetPlan, Api.Contracts.BudgetPlan.BudgetPlanDetails>();

            CreateMap<DataAccessLayer.Entities.Saving, Api.Contracts.Saving.SavingList>();
            CreateMap<DataAccessLayer.Entities.Saving, Api.Contracts.Saving.SavingDetails>();
        }
    }
}
