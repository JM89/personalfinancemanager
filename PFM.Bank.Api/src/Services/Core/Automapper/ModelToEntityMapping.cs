using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Services.Core.Automapper
{
    public class ModelToEntityMapping : Profile
    {
        public ModelToEntityMapping()
        {
            CreateMap<Api.Contracts.Bank.BankDetails, DataAccessLayer.Entities.Bank>();

            CreateMap<Api.Contracts.Country.CountryList, DataAccessLayer.Entities.Country>();
            CreateMap<Api.Contracts.Country.CountryDetails, DataAccessLayer.Entities.Country>();

            CreateMap<Api.Contracts.Currency.CurrencyList, DataAccessLayer.Entities.Currency>();
            CreateMap<Api.Contracts.Currency.CurrencyDetails, DataAccessLayer.Entities.Currency>();

            CreateMap<Api.Contracts.Account.AccountList, DataAccessLayer.Entities.Account>();
            CreateMap<Api.Contracts.Account.AccountDetails, DataAccessLayer.Entities.Account>();
        }
    }
}
