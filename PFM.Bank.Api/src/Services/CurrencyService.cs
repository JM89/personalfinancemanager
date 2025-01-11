using AutoMapper;
using PFM.Bank.Api.Contracts.Currency;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Repositories;
using Services.Core;

namespace Services
{
    public interface ICurrencyService : IBaseService
    {
        Task<List<CurrencyList>> GetCurrencies();

        Task<CurrencyDetails> GetById(int id);
    }
    
    public class CurrencyService(ICurrencyRepository repository) : ICurrencyService
    {
        public Task<List<CurrencyList>> GetCurrencies()
        {
            var currencies = repository.GetList().ToList();

            var mappedCurrencies = currencies.Select(Mapper.Map<CurrencyList>).ToList();

            mappedCurrencies.ForEach(currency =>
            {
                currency.CanBeDeleted = false;
            });

            return Task.FromResult(mappedCurrencies);
        }

        public Task<CurrencyDetails> GetById(int id)
        {
            var currency = repository.GetById(id);

            if (currency == null)
            {
                return null;
            }

            return Task.FromResult(Mapper.Map<CurrencyDetails>(currency));
        }
    }
}