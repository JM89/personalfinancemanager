using AutoMapper;

namespace PFM.Website.Services.Mappers
{
	public class ModelToResponseProfile : Profile
    {
		public ModelToResponseProfile()
		{
            CreateMap<PFM.Api.Contracts.ExpenseType.ExpenseTypeList, Models.ExpenseTypeModel>();
            CreateMap<PFM.Api.Contracts.ExpenseType.ExpenseTypeDetails, Models.ExpenseTypeModel>();
            CreateMap<PFM.Bank.Api.Contracts.Country.CountryList, Models.CountryModel>();
            CreateMap<PFM.Bank.Api.Contracts.Country.CountryDetails, Models.CountryModel>();
            CreateMap<PFM.Bank.Api.Contracts.Bank.BankList, Models.BankModel>();
            CreateMap<PFM.Bank.Api.Contracts.Bank.BankDetails, Models.BankModel>();
        }
    }
}

