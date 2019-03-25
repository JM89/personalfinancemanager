using AutoMapper;
using PFM.Services.Interfaces;
using PFM.DataAccessLayer.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using PFM.DataAccessLayer.Entities;
using PFM.Services.DTOs.Tax;

namespace PFM.Services
{
    public class TaxService : ITaxService
    {
        private readonly ITaxRepository _taxRepository;
        private readonly ISalaryRepository _salaryRepository;

        public TaxService(ITaxRepository taxRepository, ISalaryRepository salaryRepository)
        {
            this._taxRepository = taxRepository;
            this._salaryRepository = salaryRepository;
        }

        public void CreateTax(TaxDetails taxDetails)
        {
            var tax = Mapper.Map<Tax>(taxDetails);
            _taxRepository.Create(tax);
        }

        public void DeleteTax(int id)
        {
            var tax = _taxRepository.GetById(id);
            _taxRepository.Delete(tax);
        }

        public void EditTax(TaxDetails taxDetails)
        {
            var tax = Mapper.Map<Tax>(taxDetails);
            _taxRepository.Update(tax);
        }

        public TaxDetails GetById(int id)
        {
            var tax = _taxRepository.GetById(id);
            if (tax == null)
            {
                return null;
            }
            return Mapper.Map<TaxDetails>(tax);
        }

        public IList<TaxList> GetTaxes(string userId)
        {
            var taxes = _taxRepository.GetList2(x => x.Country, x => x.Currency, x => x.FrequenceOption, x => x.TaxType)
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

                mappedTaxes.Add(mappedTax);
            }

            return mappedTaxes.ToList();
        }

        public IList<TaxList> GetTaxesByType(string currentUser, int taxTypeId)
        {
            var taxes = _taxRepository.GetList().Where(x => x.TaxTypeId == taxTypeId).ToList();
            return taxes.Select(Mapper.Map<TaxList>).ToList();
        }
    }
}