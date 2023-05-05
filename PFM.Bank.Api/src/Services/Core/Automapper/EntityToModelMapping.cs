using AutoMapper;

namespace PFM.Services.Core.Automapper
{
    public class EntityToModelMapping : Profile
    {
        public EntityToModelMapping()
        {
            CreateMap<DataAccessLayer.Entities.Country, PFM.Bank.Api.Contracts.Country.CountryList>();
            CreateMap<DataAccessLayer.Entities.Country, PFM.Bank.Api.Contracts.Country.CountryDetails>();

            CreateMap<DataAccessLayer.Entities.Bank, PFM.Bank.Api.Contracts.Bank.BankList>();
            CreateMap<DataAccessLayer.Entities.Bank, PFM.Bank.Api.Contracts.Bank.BankDetails>();

            CreateMap<DataAccessLayer.Entities.Currency, PFM.Bank.Api.Contracts.Currency.CurrencyList>();
            CreateMap<DataAccessLayer.Entities.Currency, PFM.Bank.Api.Contracts.Currency.CurrencyDetails>();

            CreateMap<DataAccessLayer.Entities.Account, PFM.Bank.Api.Contracts.Account.AccountList>();
            CreateMap<DataAccessLayer.Entities.Account, PFM.Bank.Api.Contracts.Account.AccountDetails>()
                .ForMember(a => a.OwnerId, opt => opt.MapFrom(x => x.User_Id)); 
        }
    }
}
