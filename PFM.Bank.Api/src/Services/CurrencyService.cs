using AutoMapper;
using DataAccessLayer.Repositories.Interfaces;
using PFM.Bank.Api.Contracts.Currency;
using Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyService(ICurrencyRepository currencyRepository)
        {
            this._currencyRepository = currencyRepository;
        }

        public Task<List<CurrencyList>> GetCurrencies()
        {
            var currencies = _currencyRepository.GetList().ToList();

            var mappedCurrencies = currencies.Select(Mapper.Map<CurrencyList>).ToList();

            mappedCurrencies.ForEach(currency =>
            {
                currency.CanBeDeleted = false;
            });

            return Task.FromResult(mappedCurrencies);
        }

        public Task<CurrencyDetails> GetById(int id)
        {
            var currency = _currencyRepository.GetById(id);

            if (currency == null)
            {
                return null;
            }

            return Task.FromResult(Mapper.Map<CurrencyDetails>(currency));
        }
    }
}