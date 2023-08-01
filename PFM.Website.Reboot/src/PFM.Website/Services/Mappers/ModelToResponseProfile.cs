using AutoMapper;
using PFM.Website.Models;

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
            CreateMap<PFM.Bank.Api.Contracts.Bank.BankList, Models.BankListModel>();
            CreateMap<PFM.Bank.Api.Contracts.Bank.BankDetails, Models.BankEditModel>();
            CreateMap<PFM.Bank.Api.Contracts.Currency.CurrencyList, Models.CurrencyModel>();
            CreateMap<PFM.Bank.Api.Contracts.Currency.CurrencyDetails, Models.CurrencyModel>();
            CreateMap<PFM.Bank.Api.Contracts.Account.AccountList, Models.BankAccountListModel>();
            CreateMap<PFM.Bank.Api.Contracts.Account.AccountDetails, Models.BankAccountEditModel>();
        }
    }
}

