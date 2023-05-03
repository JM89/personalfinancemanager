using AutoMapper;
using PFM.Services.Interfaces;
using PFM.DataAccessLayer.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using PFM.DataAccessLayer.Entities;
using PFM.Api.Contracts.Tax;
using PFM.Services.Caches.Interfaces;
using System.Threading.Tasks;
using PFM.Services.Core.Exceptions;

namespace PFM.Services
{
    public class TaxService : ITaxService
    {
        private readonly ITaxRepository _taxRepository;
        private readonly ISalaryRepository _salaryRepository;
        private readonly ICountryCache _countryCache;
        private readonly ICurrencyCache _currencyCache;

        public TaxService(ITaxRepository taxRepository, ISalaryRepository salaryRepository, ICountryCache countryCache, ICurrencyCache currencyCache)
        {
            this._taxRepository = taxRepository;
            this._salaryRepository = salaryRepository;
            this._countryCache = countryCache;
            this._currencyCache = currencyCache;
        }

        public Task<bool> CreateTax(TaxDetails taxDetails)
        {
            var tax = Mapper.Map<Tax>(taxDetails);
            _taxRepository.Create(tax);
            return Task.FromResult(true);
        }

        public Task<bool> DeleteTax(int id)
        {
            var tax = _taxRepository.GetById(id);
            _taxRepository.Delete(tax);
            return Task.FromResult(true);
        }

        public Task<bool> EditTax(TaxDetails taxDetails)
        {
            var tax = Mapper.Map<Tax>(taxDetails);
            _taxRepository.Update(tax);
            return Task.FromResult(true);
        }

        public Task<TaxDetails> GetById(int id)
        {
            var tax = _taxRepository.GetById(id);
            if (tax == null)
            {
                throw new BusinessException("tax", "Cannot be found");
            }
            return Task.FromResult(Mapper.Map<TaxDetails>(tax));
        }

        public async Task<IList<TaxList>> GetTaxes(string userId)
        {
            var taxes = _taxRepository.GetList2(x => x.FrequenceOption, x => x.TaxType)
                            .Where(x => x.UserId == userId).ToList();
            
            var mappedTaxes = new List<TaxList>();

            foreach (var tax in taxes)
            {
                var mappedTax = Mapper.Map<TaxList>(tax);

                switch ((DataAccessLayer.Enumerations.FrequenceOption)tax.FrequenceOptionId)
                {
                    case DataAccessLayer.Enumerations.FrequenceOption.Once:
                        mappedTax.FrequenceDescription = "Once";
                        break;
                    case DataAccessLayer.Enumerations.FrequenceOption.EveryXMonths:
                        mappedTax.FrequenceDescription = $"Every {tax.Frequence} months";
                        break;
                }

                var hasSalary = _salaryRepository.GetList().Any(x => x.TaxId == tax.Id);

                mappedTax.CanBeDeleted = !hasSalary;

                var country = await _countryCache.GetById(tax.CountryId);
                mappedTax.CountryName = country.Name;

                var currency = await _currencyCache.GetById(tax.CurrencyId);
                mappedTax.CurrencySymbol = currency.Symbol;

                mappedTaxes.Add(mappedTax);
            }

            return mappedTaxes.ToList();
        }

        public Task<List<TaxList>> GetTaxesByType(string currentUser, int taxTypeId)
        {
            var taxes = _taxRepository.GetList().Where(x => x.TaxTypeId == taxTypeId).ToList();
            return Task.FromResult(taxes.Select(Mapper.Map<TaxList>).ToList());
        }
    }
}