using AutoMapper;

namespace PFM.Services.Core.Automapper
{
    public class ModelToEntityMapping : Profile
    {
        public ModelToEntityMapping()
        {
            CreateMap<PFM.Bank.Api.Contracts.Bank.BankDetails, DataAccessLayer.Entities.Bank>();

            CreateMap<PFM.Bank.Api.Contracts.Country.CountryList, DataAccessLayer.Entities.Country>();
            CreateMap<PFM.Bank.Api.Contracts.Country.CountryDetails, DataAccessLayer.Entities.Country>();

            CreateMap<PFM.Bank.Api.Contracts.Currency.CurrencyList, DataAccessLayer.Entities.Currency>();
            CreateMap<PFM.Bank.Api.Contracts.Currency.CurrencyDetails, DataAccessLayer.Entities.Currency>();

            CreateMap<PFM.Bank.Api.Contracts.Account.AccountList, DataAccessLayer.Entities.Account>();
            CreateMap<PFM.Bank.Api.Contracts.Account.AccountDetails, DataAccessLayer.Entities.Account>();
        }
    }
}
