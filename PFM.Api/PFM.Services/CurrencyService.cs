using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PFM.Services.Interfaces;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DTOs.Currency;
using PFM.DataAccessLayer.Entities;

namespace PFM.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ISalaryRepository _salaryRepository;
        private readonly IPensionRepository _pensionRepository;
        private readonly ITaxRepository _taxRepository;
        private readonly IBankAccountRepository _bankAccountRepository;

        public CurrencyService(ICurrencyRepository currencyRepository, IPensionRepository pensionRepository, ITaxRepository taxRepository, ISalaryRepository salaryRepository, IBankAccountRepository bankAccountRepository)
        {
            this._currencyRepository = currencyRepository;
            this._bankAccountRepository = bankAccountRepository;
            this._salaryRepository = salaryRepository;
            this._pensionRepository = pensionRepository;
            this._taxRepository = taxRepository;
        }

        public IList<CurrencyList> GetCurrencies()
        {
            var currencies = _currencyRepository.GetList().ToList();

            var mappedCurrencies = currencies.Select(Mapper.Map<CurrencyList>).ToList();

            mappedCurrencies.ForEach(currency =>
            {
                var hasAccount = _bankAccountRepository.GetList().Any(x => x.CurrencyId == currency.Id);
                var hasSalary = _salaryRepository.GetList().Any(x => x.CurrencyId == currency.Id);
                var hasPension = _pensionRepository.GetList().Any(x => x.CurrencyId == currency.Id);
                var hasTax = _taxRepository.GetList().Any(x => x.CurrencyId == currency.Id);

                currency.CanBeDeleted = !hasAccount && !hasSalary && !hasPension && !hasTax;
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

        public void CreateCurrency(CurrencyDetails currencyDetails)
        {
            var currency = Mapper.Map<Currency>(currencyDetails);
            _currencyRepository.Create(currency);
        }

        public void EditCurrency(CurrencyDetails currencyDetails)
        {
            var currency = _currencyRepository.GetById(currencyDetails.Id); 
            currency.Name = currencyDetails.Name;
            currency.Symbol = currencyDetails.Symbol;
            _currencyRepository.Update(currency);
        }

        public void DeleteCurrency(int id)
        {
            var currency = _currencyRepository.GetById(id);
            _currencyRepository.Delete(currency);
        }
    }
}