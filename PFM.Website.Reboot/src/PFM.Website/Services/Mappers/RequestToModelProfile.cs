using AutoMapper;

namespace PFM.Website.Services.Mappers
{
	public class RequestToModelProfile : Profile
    {
		public RequestToModelProfile()
		{
            CreateMap<Models.ExpenseTypeModel, PFM.Api.Contracts.ExpenseType.ExpenseTypeDetails>();
            CreateMap<Models.CountryModel, PFM.Bank.Api.Contracts.Country.CountryDetails>();
            CreateMap<Models.BankEditModel, PFM.Bank.Api.Contracts.Bank.BankDetails>();
            CreateMap<Models.BankListModel, PFM.Bank.Api.Contracts.Bank.BankDetails>();
            CreateMap<Models.BankAccountEditModel, PFM.Bank.Api.Contracts.Account.AccountDetails>();
            CreateMap<Models.BankAccountListModel, PFM.Bank.Api.Contracts.Account.AccountDetails>();
            CreateMap<Models.IncomeEditModel, PFM.Api.Contracts.Income.IncomeDetails>();
            CreateMap<Models.IncomeListModel, PFM.Api.Contracts.Income.IncomeList>();
            CreateMap<Models.SavingEditModel, PFM.Api.Contracts.Saving.SavingDetails>();
            CreateMap<Models.SavingListModel, PFM.Api.Contracts.Saving.SavingList>();
        }
    }
}

