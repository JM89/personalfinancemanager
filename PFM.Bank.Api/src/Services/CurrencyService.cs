using AutoMapper;
using DataAccessLayer.Repositories.Interfaces;
using PFM.Bank.Api.Contracts.Currency;
using Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyService(ICurrencyRepository currencyRepository)
        {
            this._currencyRepository = currencyRepository;
        }

        public IList<CurrencyList> GetCurrencies()
        {
            var currencies = _currencyRepository.GetList().ToList();

            var mappedCurrencies = currencies.Select(Mapper.Map<CurrencyList>).ToList();

            mappedCurrencies.ForEach(currency =>
            {
                currency.CanBeDeleted = false;
            });

            return mappedCurrencies;
        }

        public CurrencyDetails GetById(int id)
        {
            var currency = _currencyRepository.GetById(id);

            if (currency == null)
            {
                return null;
            }

            return Mapper.Map<CurrencyDetails>(currency);
        }
    }
}