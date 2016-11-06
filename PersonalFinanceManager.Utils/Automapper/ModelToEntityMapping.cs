﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Utils.Automapper
{
    public class ModelToEntityMapping : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Models.Expenditure.ExpenditureEditModel, Entities.ExpenditureModel>();
            Mapper.CreateMap<Models.Expenditure.ExpenditureListModel, Entities.ExpenditureModel>();

            Mapper.CreateMap<Models.Bank.BankListModel, Entities.BankModel>();
            Mapper.CreateMap<Models.Bank.BankEditModel, Entities.BankModel>();

            Mapper.CreateMap<Models.AtmWithdraw.AtmWithdrawListModel, Entities.AtmWithdrawModel>();
            Mapper.CreateMap<Models.AtmWithdraw.AtmWithdrawEditModel, Entities.AtmWithdrawModel>();

            Mapper.CreateMap<Models.PaymentMethod.PaymentMethodListModel, Entities.PaymentMethodModel>();

            Mapper.CreateMap<Models.Country.CountryListModel, Entities.CountryModel>();
            Mapper.CreateMap<Models.Country.CountryEditModel, Entities.CountryModel>();

            Mapper.CreateMap<Models.Income.IncomeListModel, Entities.IncomeModel>();
            Mapper.CreateMap<Models.Income.IncomeEditModel, Entities.IncomeModel>();

            Mapper.CreateMap<Models.PeriodicOutcome.PeriodicOutcomeListModel, Entities.PeriodicOutcomeModel>();
            Mapper.CreateMap<Models.PeriodicOutcome.PeriodicOutcomeEditModel, Entities.PeriodicOutcomeModel>();

            Mapper.CreateMap<Models.Frequency.FrequencyListModel, Entities.FrequencyModel>();

            Mapper.CreateMap<Models.ExpenditureType.ExpenditureTypeListModel, Entities.ExpenditureTypeModel>();
            Mapper.CreateMap<Models.ExpenditureType.ExpenditureTypeEditModel, Entities.ExpenditureTypeModel>();

            Mapper.CreateMap<Models.Currency.CurrencyListModel, Entities.CurrencyModel>();
            Mapper.CreateMap<Models.Currency.CurrencyEditModel, Entities.CurrencyModel>();

            Mapper.CreateMap<Models.Account.AccountListModel, Entities.AccountModel>();
            Mapper.CreateMap<Models.Account.AccountEditModel, Entities.AccountModel>();
        }
    }
}
