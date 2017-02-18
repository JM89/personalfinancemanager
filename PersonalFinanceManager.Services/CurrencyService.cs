using System.Collections.Generic;
using System.Linq;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Models.Currency;
using AutoMapper;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;

namespace PersonalFinanceManager.Services
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

        public IList<CurrencyListModel> GetCurrencies()
        {
            var currencies = _currencyRepository.GetList().ToList();

            var currenciesModel = currencies.Select(Mapper.Map<CurrencyListModel>).ToList();

            currenciesModel.ForEach(currency =>
            {
                var hasAccount = _bankAccountRepository.GetList().Any(x => x.CurrencyId == currency.Id);
                var hasSalary = _salaryRepository.GetList().Any(x => x.CurrencyId == currency.Id);
                var hasPension = _pensionRepository.GetList().Any(x => x.CurrencyId == currency.Id);
                var hasTax = _taxRepository.GetList().Any(x => x.CurrencyId == currency.Id);

                currency.CanBeDeleted = !hasAccount && !hasSalary && !hasPension && !hasTax;
            });

            return currenciesModel;
        }

        public CurrencyEditModel GetById(int id)
        {
            var currency = _currencyRepository.GetById(id);

            if (currency == null)
            {
                return null;
            }

            return Mapper.Map<CurrencyEditModel>(currency);
        }

        public void CreateCurrency(CurrencyEditModel currencyEditModel)
        {
            var currencyModel = Mapper.Map<CurrencyModel>(currencyEditModel);
            _currencyRepository.Create(currencyModel);
        }

        public void EditCurrency(CurrencyEditModel currencyEditModel)
        {
            var currencyModel = _currencyRepository.GetById(currencyEditModel.Id); 
            currencyModel.Name = currencyEditModel.Name;
            currencyModel.Symbol = currencyEditModel.Symbol;
            _currencyRepository.Update(currencyModel);
        }

        public void DeleteCurrency(int id)
        {
            var currencyModel = _currencyRepository.GetById(id);
            _currencyRepository.Delete(currencyModel);
        }
    }
}