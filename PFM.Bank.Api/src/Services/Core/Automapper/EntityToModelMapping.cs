using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Services.Core.Automapper
{
    public class EntityToModelMapping : Profile
    {
        public EntityToModelMapping()
        {
            CreateMap<DataAccessLayer.Entities.Country, Api.Contracts.Country.CountryList>();
            CreateMap<DataAccessLayer.Entities.Country, Api.Contracts.Country.CountryDetails>();

            CreateMap<DataAccessLayer.Entities.Bank, Api.Contracts.Bank.BankList>();
            CreateMap<DataAccessLayer.Entities.Bank, Api.Contracts.Bank.BankDetails>();

            CreateMap<DataAccessLayer.Entities.Currency, Api.Contracts.Currency.CurrencyList>();
            CreateMap<DataAccessLayer.Entities.Currency, Api.Contracts.Currency.CurrencyDetails>();

            CreateMap<DataAccessLayer.Entities.Account, Api.Contracts.Account.AccountList>();
            CreateMap<DataAccessLayer.Entities.Account, Api.Contracts.Account.AccountDetails>();
        }
    }
}
