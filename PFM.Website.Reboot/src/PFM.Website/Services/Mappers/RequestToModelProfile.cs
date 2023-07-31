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
        }
    }
}

